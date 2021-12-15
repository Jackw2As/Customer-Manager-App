using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.ValueObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository.Customer
{
    //Using NewtonSoft Json seraliser to read and write to a Json File for storing Customer List.
    //Json is used because this is a Demo Project in a real project i'd use a database probably stored on an Azure or AWS service with the data accessed through some kind of secure service.

    public class JsonCustomerRepository : ICustomerRepository
    {
        //Hard Coded File Name for storage because load/saving from sperate customer lists isn't supported.
        private static readonly string CustomersFileName = "customers.json";
        private DirectoryInfo jsonStorageDirectory;
        private string directoryName;
        private IEnumerable<CustomerValueObject> defaultCustomerList;
        public JsonCustomerRepository(string DirectoryName = "Json", IEnumerable<CustomerValueObject>? DefaultCustomerList = null)
        {

            directoryName = DirectoryName;
            if (DefaultCustomerList != null)
            {
                defaultCustomerList = DefaultCustomerList;
            }
            else
            {
                var drinks = new MockDrinkRepository().LoadDrinkTypes() as List<DrinkValueObject>;
                defaultCustomerList = new List<CustomerValueObject>
                {
                    new CustomerValueObject("Jack", "Aalders", drinks[0].Id, true),
                    new CustomerValueObject("John", "Aalders", drinks[0].Id, true),
                    new CustomerValueObject("Sarah", "Spear", drinks[0].Id, false)
                };
            }
            jsonStorageDirectory = ConstructJsonStorageDirectory();
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

        public async Task<IEnumerable<CustomerValueObject>> LoadCustomersAsync()
        {
            FileInfo? file = null;
            var files = jsonStorageDirectory.GetFiles(CustomersFileName).ToList();
            if (files.Count > 0)
            {
                file = files?.First();
            }

            if (file == null)
            {
                //Generate some random test values
                return defaultCustomerList;
            }

            List<CustomerValueObject> customers = new();

            //List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText());
            using (FileStream reader = file.OpenRead())
            {
                var DeserializationResults = await JsonSerializer.DeserializeAsync(reader, typeof(List<CustomerValueObject>)) as List<CustomerValueObject>;
                if (DeserializationResults != null)
                {
                    customers.AddRange(DeserializationResults);
                }
            }
            return customers;
        }

        public async Task SaveCustomerAsync(IEnumerable<CustomerValueObject> customers)
        {
            using (var file = File.Create(jsonStorageDirectory.FullName + "/" + CustomersFileName))
            {
                await JsonSerializer.SerializeAsync(file, customers);
            }
        }

        public void DeleteStorageFile()
        {
            var files = jsonStorageDirectory.GetFiles(CustomersFileName);
            foreach (FileInfo file in files)
            {
                File.Delete(file.FullName);
            }
        }
    }
}
