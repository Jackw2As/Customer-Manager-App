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
    public class JsonCustomerRepo : ICustomerRepository
    {
        FileInfo PersistentStorage { get; set; }
        DirectoryInfo ApplicationFolder { get; init; }
        DirectoryInfo SaveFolder { get; init; }
        List<CustomerEntity>? Entities { get; set; }
        FileStream SaveFileStream { get; set; }

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
        }

       

        public async void Create(CustomerEntity Model)
        {
            if(Entities == null)
            {
                Entities = await DeserializeFile();
            }

            if(Entities.Exists(_ => _ == Model))
            {
                throw new ArgumentException("Trying to Duplicate Customer Entry!");
            }
            Entities.Add(Model);
            SerializeFile();
        }

        
        public void Delete(CustomerEntity Model)
        {

            if(Entities == null)
            {
                throw new ArgumentNullException(nameof(Entities), "File Not Read or Doesn't Exist!");
            }

            var sucessful = Entities.Remove(Model);

            if(!sucessful)
            {
                throw new ArgumentException("CustomerEntity trying to be removed from the file doesn't exist", nameof(Model));
            }
        }

        public void DeleteAll()
        {
            Entities = null;
            if (SaveFileStream != null)
            { 
                SaveFileStream.Dispose(); 
            }
            PersistentStorage.Delete();
            PersistentStorage.Refresh();
        }

        public async Task<CustomerEntity> Read(string id)
        {
            while(Entities == null)
            {
                Entities = await DeserializeFile();
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
                Entities = await DeserializeFile();
            }
            var list = Entities.ToList();
            return list;
        }

        public void Update(CustomerEntity Model)
        {
            if(Entities == null)
            {
                throw new ArgumentNullException(nameof(Entities), "Trying to Update, but no Entities exist on File!(Or file hasn't been read)");
            }
            var successful = Entities.Remove(Model);
            if(successful)
            {
                Entities.Add(Model);
                SerializeFile();
            }
            throw new NullReferenceException("List Does Not contain value trying to be updated!");
        }

        async void SerializeFile()
        {
            if (SaveFileStream == null)
            {
                SaveFileStream = PersistentStorage.Create();
            }
            await JsonSerializer.SerializeAsync(SaveFileStream, Entities);
        }

        private async Task<List<CustomerEntity>?> DeserializeFile()
        {
            try
            {
                if(SaveFileStream == null)
                {
                    SaveFileStream = PersistentStorage.Create();
                }

                List<CustomerEntity>? customers;

                customers = await JsonSerializer.DeserializeAsync(SaveFileStream, typeof(List<CustomerEntity>))
                        as List<CustomerEntity>;
                return customers;
            }
            catch(JsonException ex)
            {
                return new();
            }
        }
    }
}
