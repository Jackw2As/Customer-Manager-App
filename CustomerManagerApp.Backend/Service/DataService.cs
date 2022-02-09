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
                var drinks = await drinkRepository.ReadAll();
                var drinkId = drinks.First().ID;
                if (drinks == null) throw new ArgumentNullException($"{nameof(drinks)} is null. Something went wrong with {nameof(drinkRepository.ReadAll)} method");

                List<CustomerEntity> list = new List<CustomerEntity>()
            {
                new CustomerEntity(Guid.NewGuid().ToString(), "Jack", "Aalders", drinkId),
                new CustomerEntity(Guid.NewGuid().ToString(), "John", "Aalders", drinkId),
                new CustomerEntity(Guid.NewGuid().ToString(), "Sarah", "Aalders", drinkId)
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
        private async Task<List<DrinkEntity>> GetDrinksFromRepo(IDrinkRepository repository) => await repository.ReadAll();

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
            customerList.Remove(customer);
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
                var databaseList = await customerRepository.ReadAll();
                Parallel.ForEach(customerList, (item) => DatabaseRequestHandler(item));

                void DatabaseRequestHandler(CustomerEntity item)
                {
                    if (databaseList.Contains(item))
                    {
                        customerRepository.Update(item);
                    }
                    else
                    {
                        customerRepository.Create(item);
                    }
                }
            }
          catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            
        }
        private async void LoadCustomersFromRepository()
        {
            customerList.Clear();
            var list = await customerRepository.ReadAll();
            customerList.AddRange(list);
        }

        #endregion


        //
        //  Saving Features
        //
        #region SavingFeatures
        private System.Timers.Timer SaveTimer = new();
        public async void Refresh()
        {
            await Task.Factory.StartNew(() => SaveCustomersToRepository());
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
