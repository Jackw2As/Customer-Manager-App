using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiredBrainCoffee_Customer_Manager_App.Model;

namespace WiredBrainCoffee_Customer_Manager_App.Services.CustomerDataLoader
{
    public interface ICustomerDataLoaderService
    {
        Task<IEnumerable<Customer>> LoadCustomersAsync();

        Task SaveCustomerAsync(IEnumerable<Customer> customers);
    }
}
