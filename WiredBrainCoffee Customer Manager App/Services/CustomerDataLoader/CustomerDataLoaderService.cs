using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using WiredBrainCoffee_Customer_Manager_App.Model;


namespace WiredBrainCoffee_Customer_Manager_App.Services.CustomerDataLoader
{
    //Using NewtonSoft Json seraliser to read and write to a Json File for storing Customer List.
    //Json is used because this is a Demo Project in a real project i'd use a database probably stored on an Azure or AWS service with the data accessed through some kind of secure service.

    class CustomerDataJsonLoader : ICustomerDataLoaderService
    {
        //Hard Coded File Name for storage because load/saving from sperate customer lists isn't supported.
        private static readonly string CustomersFileName = "customers.json";
        private static readonly StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;

        public async Task<IEnumerable<Customer>> LoadCustomersAsync()
        {
            var file = await LocalFolder.TryGetItemAsync(CustomersFileName) as StorageFile;

            if (file is null)
            {
                //Generate some random test values
                return new List<Customer>
                {
                    new Customer("Jack", "Aalders", true),
                    new Customer("John", "Aalders", true),
                    new Customer("Sarah", "Spear", false)
                };
            }

            List<Customer> customers = null;

            //List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText());
            using (StreamReader reader = File.OpenText(file.Path))
            {
                JsonSerializer serializer = new JsonSerializer();
               customers = (List<Customer>)serializer.Deserialize(reader, typeof(List<Customer>));
            }
            return customers;
        }

        public async Task SaveCustomerAsync(IEnumerable<Customer> customers)
        {
            using (StreamWriter file = File.CreateText(LocalFolder.Path + "/" + CustomersFileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, customers);
            }
        }
    }
}
