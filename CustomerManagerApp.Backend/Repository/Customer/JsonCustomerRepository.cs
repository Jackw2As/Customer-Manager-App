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
                defaultCustomerList = DefaultCustomerList;
            }

            jsonStorageDirectory = ConstructJsonStorageDirectory();
        }

        static private bool FileLocked = false;

        public async Task<IEnumerable<CustomerEntity>> LoadCustomersAsync()
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
                    return CreateDefaultData();
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
        public async Task SaveCustomerAsync(IEnumerable<CustomerEntity> customers)
        {
            try
            {
                while (FileLocked)
                {
                    await Task.Delay(100);
                }
                FileLocked = true;

                using (FileStream file = File.Create(jsonStorageDirectory.FullName + "/" + CustomersFileName))
                {
                    await JsonSerializer.SerializeAsync(file, customers);
                }
                FileLocked = false;
                return;
            }
            catch (IOException ex)
            {

            }
        }
        public void DeleteStorageFile()
        {
            try
            {
                var files = jsonStorageDirectory.GetFiles(CustomersFileName);
                foreach (FileInfo file in files)
                {
                    File.Delete(file.FullName);
                }
            }
            catch(IOException ex)
            {
            }
        }




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

        private IEnumerable<CustomerEntity>? defaultCustomerList;
        private IEnumerable<CustomerEntity> CreateDefaultData()
        {
            if (defaultCustomerList == null) { 
                defaultCustomerList = new List<CustomerEntity>(); 
            }

            if (defaultCustomerList.Count() < 1)
            {
                var drinks = new MockDrinkRepository().LoadDrinkTypes() as List<DrinkEntity>;
                if (drinks == null)
                {
                    throw new ArgumentNullException("Drinks weren't found in the repository!");
                }
                defaultCustomerList = new List<CustomerEntity>
                    {
                        new CustomerEntity("Jack", "Aalders", drinks[0].Id, true),
                        new CustomerEntity("John", "Aalders", drinks[0].Id, true),
                        new CustomerEntity("Sarah", "Spear", drinks[0].Id, false)
                    };
            }

            return defaultCustomerList;
        }
    }
}
