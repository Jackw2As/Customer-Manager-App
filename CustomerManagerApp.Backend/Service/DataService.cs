using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Backend.Repository.Customer;
using CustomerManagerApp.Backend.Repository.Drink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace CustomerManagerApp.Backend.Service
{
    public class DataService
    {
        private System.Timers.Timer SaveTimer;
        private readonly ICustomerRepository customerRepository;

        private readonly IDrinkRepository drinkRepository;

        /// <summary>
        /// Handles CRUD Implementation for data that is stored persistently. 
        /// </summary>
        public DataService(ICustomerRepository CustomerRepository, IDrinkRepository DrinkRepository)
        {
            drinkRepository = DrinkRepository;
            customerRepository = CustomerRepository;
            SaveTimer = new System.Timers.Timer(10000); //10 Second Timer
            SaveTimer.Elapsed += Save;
        }

        private async void Save(object sender, ElapsedEventArgs e)
        {
            await SaveToRepository();
        }

        public DataService()
        {
            drinkRepository = new MockDrinkRepository();
            customerRepository = new JsonCustomerRepository();
        }

        //Drinks
        public async Task<IEnumerable<DrinkEntity>> GetDrinksAsync()
        {
            return await drinkRepository.LoadDrinkTypesAsync();
        }

        public IEnumerable<DrinkEntity> GetDrinks()
        {
            var drinks = drinkRepository.LoadDrinkTypesAsync().Result;
            return drinks;
        }

        //Customers

        public async Task<IEnumerable<CustomerEntity>> GetCustomersAsync()
        {
            return await getCustomerListAsync();
        }

        private async Task AddCustomerAsync(CustomerEntity customer)
        {
            var customers = await getCustomerListAsync();

            if (customers.Find(c => c == customer) != null)
            {
                throw new Exception("Can't 'Add' item which already exists.");
            }

            customers.Add(customer);
        }

        public async Task RemoveCustomerAsync(CustomerEntity customer)
        {
            var customers = await getCustomerListAsync();
            if (customers.Find(c => c == customer) != null)
            {
                throw new ArgumentException("Deleting Customer that does not exist in Database!.");
            }

            customers.Remove(customer);
        }

        public async Task UpdateCustomerAsync(CustomerEntity customer)
        {
            var customers = await getCustomerListAsync();
            var edit = customers.Find(c => c == customer);
            if (edit == null)
            {
                await AddCustomerAsync(customer);
                return;
            }

            edit.FirstTime = customer.FirstTime;
            edit.DrinkID = customer.DrinkID;
            edit.FirstName = customer.FirstName;
            edit.LastName = customer.LastName;
            edit.IsDeveloper = customer.IsDeveloper;
        }

        private List<CustomerEntity>? CustomerList;
        private async Task<List<CustomerEntity>> getCustomerListAsync()
        {
            if (CustomerList == null)
            {
                var customers = await customerRepository.LoadCustomersAsync() as List<CustomerEntity>;
                if (customers == null)
                {
                    throw new Exception("Customer Data is Corrupted. Couldn't read it.");
                }
                return customers;
            }

            return CustomerList;
        }

        private async Task SaveToRepository()
        {
            if (CustomerList != null)
            {
                await customerRepository.SaveCustomerAsync(CustomerList);
                CustomerList = null;
            }
        }
    }
}
