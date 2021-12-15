using CustomerManagerApp.Backend.Repository.Customer;
using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.ValueObjects;
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
        public async Task<IEnumerable<DrinkValueObject>> GetDrinksAsync()
        {
            return await drinkRepository.LoadDrinkTypesAsync();
        }

        //Customers

        public async Task<IEnumerable<CustomerValueObject>> GetCustomersAsync()
        {
            return await customerRepository.LoadCustomersAsync();
        }

        public async Task AddCustomerAsync(CustomerValueObject customer)
        {
            var customers = await getCustomerListAsync();

            if (customers.Find(c => c == customer) != null)
            {
                throw new Exception("Trying to Add something that already exists. Use Update Method instead or delete the object first.");
            }

            customers.Add(customer);
            await customerRepository.SaveCustomerAsync(customers);
        }

        public async Task RemoveCustomerAsync(CustomerValueObject customer)
        {
            var customers = await getCustomerListAsync();

            var isSuccessful = customers.Remove(customer);
            if(isSuccessful == false)
            {
                throw new Exception("Deleting Customer that does not exist in Database!.");
            }
        }

        public async Task UpdateCustomerAsync(CustomerValueObject customer)
        {
            var customers = await getCustomerListAsync();
            var edit = customers.Find(c => c == customer);
            if (edit == null)
            {
                throw new Exception("Updating Customer that doesn't exist. Call Add Customer Instead");
            }

            edit.FirstTime = customer.FirstTime;
            edit.DrinkID = customer.DrinkID;
            edit.FirstName = customer.FirstName;
            edit.LastName = customer.LastName;
            edit.IsDeveloper = customer.IsDeveloper;

            await customerRepository.SaveCustomerAsync(customers);
        }

        private async Task<List<CustomerValueObject>> getCustomerListAsync()
        {
            var customers = await customerRepository.LoadCustomersAsync() as List<CustomerValueObject>;
            if (customers == null)
            {
                throw new Exception("Customer Data is Corrupted. Couldn't read it.");
            }
            return customers;
        }
    }
}
