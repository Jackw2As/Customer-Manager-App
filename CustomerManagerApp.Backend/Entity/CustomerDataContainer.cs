using CustomerManagerApp.Backend.Model;
using CustomerManagerApp.Backend.Services.CustomerDataLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Entity
{
    public class CustomerDataContainer : IDisposable
    {
        private readonly ICustomerDataLoaderService dataLoader;


#pragma warning disable CS8618 
        public CustomerDataContainer(ICustomerDataLoaderService DataLoader)
#pragma warning restore CS8618
        {
            dataLoader = DataLoader;
            HardLoad();
        }

        private List<Customer> Customers { get; set; } = new();

        ///<summary>
        /// Save Data to Persistant Storage.
        /// Use Save() for normal usage.
        /// </summary>
        public async void  HardSave()
        {
            await dataLoader.SaveCustomerAsync(Customers);
            HardLoad();
        }

        private async void HardLoad()
        {
            Customers = await dataLoader.LoadCustomersAsync() as List<Customer> ?? new();
        }

        public void Save(Customer customer)
        {
           var check = Customers.Find(c => c == customer);
            if (check != null)
            {
                check.DrinkID = customer.DrinkID;
                check.LastName = customer.LastName;
                check.FirstName = customer.FirstName;
                check.IsDeveloper = customer.IsDeveloper;
                check.FirstTime = customer.FirstTime;
            }
            else
            {
                Customers.Add(customer);
            }
        }
        public void Save(IEnumerable<Customer> customers)
        {
            foreach (var customer in customers)
            {
                Save(customer);
            }
        }

        

        public List<Customer> Load()
        {
            if(Customers.Count < 1)
            {
                HardLoad();
            }
            return Customers;
        }


        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    HardSave();
                }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                Customers = null;
#pragma warning restore CS8625
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
