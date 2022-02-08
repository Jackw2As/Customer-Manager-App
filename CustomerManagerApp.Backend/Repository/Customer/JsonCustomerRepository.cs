using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Backend.Repository.Drink;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository.Customer
{
    //Using Json Serialiser to Serialise Customer Data to store on the computer FileSystem in a Json file format.
    public class JsonCustomerRepository : ICustomerRepository
    {
        //Hard Coded File Name for storage because load/saving from seperate customer lists isn't supported.
        private static readonly string CustomersFileName = "customers.json";
        private DirectoryInfo jsonStorageDirectory;
        private string directoryName;

        public JsonCustomerRepository()
        {
            directoryName = "Json";
            jsonStorageDirectory = ConstructJsonStorageDirectory();
        }

        public JsonCustomerRepository(string DirectoryName = "Json", IEnumerable<CustomerEntity>? DefaultCustomerList = null)
        {

            directoryName = DirectoryName;
            if (DefaultCustomerList != null)
            {
                defaultCustomerList = DefaultCustomerList.ToList();
            }

            jsonStorageDirectory = ConstructJsonStorageDirectory();
        }

        static private bool FileLocked = false;


        private DirectoryInfo ConstructJsonStorageDirectory()
        {
            DirectoryInfo applicationRootDirectory = new(Directory.GetCurrentDirectory());

            DirectoryInfo? dataDirectory = applicationRootDirectory.GetDirectories("Data").FirstOrDefault();
            if (dataDirectory == null)
            {
                return CreateStorageDirectories();
            }

            DirectoryInfo? _jsonStorageDirectory = dataDirectory.GetDirectories(directoryName).FirstOrDefault();
            if (_jsonStorageDirectory == null)
            {
                return CreateStorageDirectories();
            }

            return _jsonStorageDirectory;
        }

        private DirectoryInfo CreateStorageDirectories()
        {
            var applicationFolderPath = Directory.GetCurrentDirectory();
            Directory.CreateDirectory(applicationFolderPath + "/Data");
            Directory.CreateDirectory(applicationFolderPath + "/Data/Images");
            return Directory.CreateDirectory(applicationFolderPath + "/Data/" + directoryName);
        }

        private List<CustomerEntity>? defaultCustomerList;
        private async Task<List<CustomerEntity>> CreateDefaultData()
        {
            if (defaultCustomerList == null) { 
                defaultCustomerList = new List<CustomerEntity>(); 
            }

            if (defaultCustomerList.Count() < 1)
            {
                var drinks = await new MockDrinkRepository().ReadAll();

                if (drinks == null)
                {
                    throw new ArgumentNullException("Drinks weren't found in the repository!");
                }
                defaultCustomerList = new List<CustomerEntity>
                    {
                        new CustomerEntity( Guid.NewGuid().ToString(), "Jack", "Aalders", drinks[0].ID, true),
                        new CustomerEntity( Guid.NewGuid().ToString(), "John", "Aalders", drinks[0].ID, true),
                        new CustomerEntity( Guid.NewGuid().ToString(), "Sarah", "Spear", drinks[0].ID, false)
                    };
            }

            return defaultCustomerList;
        }

        public async void  Create(CustomerEntity Model)
        {
            //TODO Fix this so it doesn't remove all the prior values.
            try
            {
                while (FileLocked)
                {
                    await Task.Delay(100);
                }
                FileLocked = true;

                using (FileStream file = File.Create(jsonStorageDirectory.FullName + "/" + CustomersFileName))
                {
                    await JsonSerializer.SerializeAsync(file, Model);
                }
                FileLocked = false;
                return;
            }
            catch (IOException ex)
            {

            }
        }

        public Task<CustomerEntity> Read(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CustomerEntity>> ReadAll()
        {
            List<CustomerEntity> customers = new();

            try
            {
                while (FileLocked)
                {
                    await Task.Delay(100);
                }
                FileLocked = true;

                FileInfo? file = null;
                var files = jsonStorageDirectory.GetFiles(CustomersFileName).ToList();
                if (files.Count > 0)
                {
                    file = files?.First();
                }

                if (file == null)
                {
                    FileLocked = false;
                    return await CreateDefaultData();
                }

                //List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText());
                using (FileStream reader = file.OpenRead())
                {
                    var DeserializationResults = await JsonSerializer.DeserializeAsync(reader, typeof(List<CustomerEntity>)) as List<CustomerEntity>;
                    if (DeserializationResults != null)
                    {
                        customers.AddRange(DeserializationResults);
                    }
                }

                FileLocked = false;
                return customers;
            }
            catch (IOException ex)
            {

            }
            return customers;
        }

        public void Update(CustomerEntity Model)
        {
            throw new NotImplementedException();
        }

        public void Delete(CustomerEntity Model)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            try
            {
                var files = jsonStorageDirectory.GetFiles(CustomersFileName);
                foreach (FileInfo file in files)
                {
                    File.Delete(file.FullName);
                }
            }
            catch (IOException ex)
            {
            }
        }
    }
}
