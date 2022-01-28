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
        private static IDrinkRepository drinkRepository;
        private static ICustomerRepository customerRepository;
        public async static Task<DataService> CreateDataServiceObjectAsync()
        {
            if (drinkRepository == null)
            {
                drinkRepository = new MockDrinkRepository();
            }
            if (customerRepository == null)
            {
                customerRepository = await setupMockCustomerRepository();
            }

            return new DataService();

            static async Task<MockCustomerRepository> setupMockCustomerRepository()
            {
                var drinks = await drinkRepository.LoadDrinkTypesAsync();
                var drinkId = drinks.First().Id;
                if (drinks == null) throw new ArgumentNullException($"{nameof(drinks)} is null. Something went wrong with {nameof(drinkRepository.LoadDrinkTypesAsync)} method");

                List<CustomerEntity> list = new List<CustomerEntity>()
            {
                new CustomerEntity("Jack", "Aalders", drinkId),
                new CustomerEntity("John", "Aalders", drinkId),
                new CustomerEntity("Sarah", "Aalders", drinkId)
            };

                return new MockCustomerRepository(list);
            }
        }



        private DataService()
        {
            //CreateTimer();
        }


        //
        //  Drinks
        //

        #region Drinks
        
        private readonly List<DrinkEntity> drinkList = new();
        
        public async Task<List<DrinkEntity>> GetDrinksAsync()
        {
            if(drinkList.Any() == false)
            {
                drinkList.AddRange(await GetDrinksFromRepo(drinkRepository));
            }

            return drinkList;
        }
        private async Task<List<DrinkEntity>> GetDrinksFromRepo(IDrinkRepository repository)
        {
           var drinks = await repository.LoadDrinkTypesAsync();
           return drinks.ToList();
        }

        #endregion


        //
        //  Customers
        //
        #region Customers
        
        private readonly List<CustomerEntity> customerList = new();

        public List<CustomerEntity> GetCustomerList()
        {
            if(customerList.Any() == false)
            {
                LoadCustomersFromRepository();
            }

            return customerList;
        }

        public void AddCustomerToList(CustomerEntity customer)
        {
            var matches = customerList.Where(listValue => listValue == customer);
            if (matches != null && matches.Count() > 0)
            {
                foreach (var customerEntity in matches)
                {
                    customerList.Remove(customerEntity);
                }
            }
            customerList.Add(customer);
        }

        public void RemoveCustomerFromList(CustomerEntity customer)
        {
            customerList.Remove(customer);
        }

        private async void SaveCustomersToRepository()
        {
            try
            {
                await customerRepository.SaveCustomerAsync(customerList);
            }
          catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async void LoadCustomersFromRepository()
        {
            customerList.Clear();
            var list = await customerRepository.LoadCustomersAsync();
            customerList.AddRange(list);
        }

        #endregion


        //
        //  Saving Features
        //
        #region SavingFeatures
        private System.Timers.Timer SaveTimer = new();
        public void Refresh()
        {
            LoadCustomersFromRepository();
        }

        private void CreateTimer()
        {
            SaveTimer = new(10000); //10 Second Timer
            
            SaveTimer.Elapsed += AutoSave;
            SaveTimer.AutoReset = true;
            SaveTimer.Enabled = true;
        }

        private void AutoSave(object sender, ElapsedEventArgs e)
        {
            SaveCustomersToRepository();
        }

        #endregion

    }
}
