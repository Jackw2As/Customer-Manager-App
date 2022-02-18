using CustomerManagerApp.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Security.AccessControl;

namespace CustomerManagerApp.Backend.Repository.Customer
{

    public class JsonCustomerRepo : QueryBaseClass, ICustomerRepository
    {
        FileInfo PersistentStorage { get; set; }
        DirectoryInfo ApplicationFolder { get; init; }
        DirectoryInfo SaveFolder { get; init; }
        List<CustomerEntity>? Entities { get; set; }
        FileStream? SaveFileStream { get; set; }

        public JsonCustomerRepo(string FileName)
        {
            // Setup Directories
            DirectoryInfo applicationRootDirectory = new(Directory.GetCurrentDirectory());
            ApplicationFolder = applicationRootDirectory.CreateSubdirectory("Customer Manager Application");
            SaveFolder = ApplicationFolder.CreateSubdirectory("Save");

            // Get/Create File Store
            string filepath = $"{SaveFolder.FullName}/{FileName}.json";
            PersistentStorage = new(filepath);
            if (!PersistentStorage.Exists)
            {
               SaveFileStream = PersistentStorage.Create();
            }

            //Setup Base Class
            base.WriteRequest += HandleWriteRequest;
            base.ReadRequest += HandleReadRequest;
            base.DeleteRequest += HandleDeleteRequest;
        }

        public JsonCustomerRepo()
        {
            // Setup Directories
            DirectoryInfo applicationRootDirectory = new(Directory.GetCurrentDirectory());
            ApplicationFolder = applicationRootDirectory.CreateSubdirectory("Customer Manager Application");
            SaveFolder = ApplicationFolder.CreateSubdirectory("Save");

            // Get/Create File Store
            string filepath = $"{SaveFolder.FullName}/customer.json";
            PersistentStorage = new(filepath);
            if (!PersistentStorage.Exists)
            {
                SaveFileStream = PersistentStorage.Create();
            }

            //Setup Base Class
            base.WriteRequest += HandleWriteRequest;
            base.ReadRequest += HandleReadRequest;
            base.DeleteRequest += HandleDeleteRequest;
        }

       

        public async void Create(CustomerEntity Model)
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

        
        public async void Delete(CustomerEntity Model)
        {
            while(Entities == null)
            {
                await EnqueueQuery(new(QueueRequest.Read));
            }

            await EnqueueQuery(new(QueueRequest.Delete, Model));
            return;
        }

        public async void DeleteAll()
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

        public async void Update(CustomerEntity Model)
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
                if (SaveFileStream != null)
                {
                    SaveFileStream.Dispose();
                }
                PersistentStorage.Delete();
                PersistentStorage.Refresh();
            }
            else
            {
                while(Entities == null)
                {
                    await EnqueueQuery(new(QueueRequest.Read));
                }
                var sucessful = Entities.Remove(query.Customer);

                if (!sucessful)
                {
                    throw new ArgumentException("CustomerEntity trying to be removed from the file doesn't exist", nameof(query.Customer));
                }
            }
            return;
        }
        private async Task HandleReadRequest(QueueQuery query) => Entities = await DeserializeFile();
        private async Task HandleWriteRequest(QueueQuery query) => await SerializeFile();

        async Task SerializeFile()
        {
            try
            {
                if (SaveFileStream == null)
                {
                    SaveFileStream = PersistentStorage.Create();
                }

                while (SaveFileStream.CanWrite == false)
                {
                    Thread.Sleep(500);
                }

                await JsonSerializer.SerializeAsync(SaveFileStream, Entities);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(500);
                await EnqueueQuery(new(QueueRequest.Write));
            }

            return;
        }
        private async Task<List<CustomerEntity>?> DeserializeFile()
        {
            try
            {
                if(SaveFileStream == null)
                {
                    SaveFileStream = PersistentStorage.Create();
                }

                while (SaveFileStream.CanRead == false)
                {
                    Thread.Sleep(500);
                }

                List<CustomerEntity>? customers;

                customers = await JsonSerializer.DeserializeAsync(SaveFileStream, typeof(List<CustomerEntity>))
                        as List<CustomerEntity>;
                return customers;
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