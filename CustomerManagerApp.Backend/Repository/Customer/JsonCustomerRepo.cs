using CustomerManagerApp.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CustomerManagerApp.Backend.Repository.Customer
{
    public class JsonCustomerRepo : QueryBaseClass, ICustomerRepository
    {
        FileInfo SaveFile { get; set; }
        List<CustomerEntity>? Entities { get; set; }
        
        public JsonCustomerRepo() : this("save")
        {

        }

        public JsonCustomerRepo(string fileName)
        {
            SaveFile = FindSaveFile(fileName);

            base.WriteRequest += HandleWriteRequest;
            base.ReadRequest += HandleReadRequest;
            base.DeleteRequest += HandleDeleteRequest;
        }

        private FileInfo FindSaveFile(string fileName)
        {
            //Find the Folder the File is located in
            string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var programFolder = Directory.CreateDirectory($"{documentsFolder}/Jacks Customer Manager Program");

            var savelocation = programFolder.CreateSubdirectory("saves");

            var files = savelocation.GetFiles(fileName);

            if(files != null)
            {
                foreach (var item in files)
                {
                    //return the first item that isn't null
                    if (item != null)
                    { 
                        if(item.Exists) return item; 
                    }
                }
            }

            //No Existing File Was Found
            var file = new FileInfo($"{savelocation.FullName}/{fileName}.json");
            if(!file.Exists)
            {
                file.Create();
            }
            return file;
        }
       

        public async Task Create(CustomerEntity Model)
        {
            while(Entities == null)
            {
               Queue.Enqueue(new(QueueRequest.Read));
               await RunQueueAsync();
            }

            if(Entities.Exists(_ => _ == Model))
            {
                throw new ArgumentException("Trying to Duplicate Customer Entry!");
            }
            Entities.Add(Model);
            Queue.Enqueue(new(QueueRequest.Write));
            await RunQueueAsync();

            return;
        }

        
        public async Task Delete(CustomerEntity Model)
        {
            while(Entities == null)
            {
                await EnqueueQuery(new(QueueRequest.Read));
            }

            await EnqueueQuery(new(QueueRequest.Delete, Model));
            return;
        }

        public async Task DeleteAll()
        {
            await EnqueueQuery(new(QueueRequest.Delete));
            return;
        }

        public async Task<CustomerEntity> Read(string id)
        {
            while(Entities == null)
            {
                await EnqueueQuery(new(QueueRequest.Read));
            }

            var customer = Entities.First(customer => customer.ID == id);
            if(customer == null)
            {
                throw new ArgumentNullException(nameof(customer),
                    $"Either Entity: {nameof(id)} did not exist, or the List is broken");
            }

            return customer;
        }

        public async Task<List<CustomerEntity>> ReadAll()
        {
            while (Entities == null)
            {
                await EnqueueQuery(new(QueueRequest.Read));
            }
            var list = Entities.ToList();
            return list;
        }

        public async Task Update(CustomerEntity Model)
        {
            while(Entities == null)
            {
                await EnqueueQuery(new(QueueRequest.Read));
            }
            var successful = Entities.Remove(Model);
            if(successful)
            {
                Entities.Add(Model);
                await EnqueueQuery(new(QueueRequest.Write));
                return;
            }
            throw new NullReferenceException("List Does Not contain value trying to be updated!");
        }
        private async Task HandleDeleteRequest(QueueQuery query)
        {
            if (query.Customer == null)
            {
                Entities = null;
                SaveFile.Delete();
                return;
            }

            while (Entities == null)
            {
                await EnqueueQuery(new(QueueRequest.Read));
            }
            var sucessful = Entities.Remove(query.Customer);

            if (!sucessful)
            {
                throw new ArgumentException("CustomerEntity trying to be removed from the file doesn't exist", nameof(query.Customer));
            }
        }
        private async Task HandleReadRequest(QueueQuery query) => Entities = await DeserializeFile();
        private async Task HandleWriteRequest(QueueQuery query) => await SerializeFile();

        private async Task SerializeFile()
        {
            try
            {
                using (var fileStream = SaveFile.OpenWrite())
                {
                    if(fileStream != null)
                    {
                        while (fileStream.CanWrite == false)
                        {
                            Thread.Sleep(500);
                        }
                        await JsonSerializer.SerializeAsync(fileStream, Entities);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(500);
                await EnqueueQuery(new(QueueRequest.Write));
            }
        }
        private async Task<List<CustomerEntity>?> DeserializeFile()
        {
            try
            {
                using (var fileStream = SaveFile.OpenRead())
                {
                    if (fileStream == null) throw new NullReferenceException(nameof(fileStream));

                    while (fileStream.CanRead == false)
                    {
                        Thread.Sleep(500);
                    }

                    return await JsonSerializer.DeserializeAsync
                        (fileStream, typeof(List<CustomerEntity>)) 
                        as List<CustomerEntity>;
                }
            }
            catch(JsonException ex)
            {
                Console.WriteLine(ex.Message);
                return new();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(500);
                return await DeserializeFile();
            }
        }
    }
}