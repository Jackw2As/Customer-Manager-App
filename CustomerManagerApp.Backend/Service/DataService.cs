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
        private readonly static IDrinkRepository drinkRepository;
        private readonly static ICustomerRepository customerRepository;

        static DataService()
        {
            drinkRepository = new MockDrinkRepository();
            customerRepository = new MockCustomerRepository(drinkRepository);
        }


        public DataService()
        {
            //CreateTimer();
            return;
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

        private static readonly List<CustomerEntity> customerList = new();

        public async Task<List<CustomerEntity>> GetCustomerList()
        {
            if(customerList.Any() == false)
            {
                await LoadCustomersFromRepository();
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

        private async Task SaveCustomersToRepository()
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
        private async Task LoadCustomersFromRepository()
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
        public async Task Refresh()
        {
            await SaveCustomersToRepository();
            await LoadCustomersFromRepository();
        }

        private void CreateTimer()
        {
            SaveTimer = new(10000); //10 Second Timer
            
            SaveTimer.Elapsed += AutoSave;
            SaveTimer.AutoReset = true;
            SaveTimer.Enabled = true;
        }

        private void AutoSave(object? sender, ElapsedEventArgs e)
        {
           Task.Run(() => SaveCustomersToRepository());
        }

        #endregion

    }
}
