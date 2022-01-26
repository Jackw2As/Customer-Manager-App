using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Backend.Repository.Customer;
using CustomerManagerApp.Backend.Repository.Drink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Service
{
    public class DataService
    {
        private readonly ICustomerRepository customerRepository;

        private readonly IDrinkRepository drinkRepository;

        /// <summary>
        /// Handles CRUD Implementation for data that is stored persistently. 
        /// </summary>
        public DataService(ICustomerRepository CustomerRepository, IDrinkRepository DrinkRepository)
        {
            drinkRepository = DrinkRepository;
            customerRepository = CustomerRepository;
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
            return await customerRepository.LoadCustomersAsync();
        }

        public async Task AddCustomerAsync(CustomerEntity customer)
        {
            var customers = await getCustomerListAsync();

            if (customers.Find(c => c == customer) != null)
            {
                throw new Exception("Trying to Add something that already exists. Use Update Method instead or delete the object first.");
            }

            customers.Add(customer);
            await customerRepository.SaveCustomerAsync(customers);
        }

        public async Task RemoveCustomerAsync(CustomerEntity customer)
        {
            var customers = await getCustomerListAsync();

            var isSuccessful = customers.Remove(customer);
            if(isSuccessful == false)
            {
                throw new ArgumentException("Deleting Customer that does not exist in Database!.");
            }
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

            await customerRepository.SaveCustomerAsync(customers);
        }

        private async Task<List<CustomerEntity>> getCustomerListAsync()
        {
            var customers = await customerRepository.LoadCustomersAsync() as List<CustomerEntity>;
            if (customers == null)
            {
                throw new Exception("Customer Data is Corrupted. Couldn't read it.");
            }
            return customers;
        }
    }
}
