using CustomerManagerApp.Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Services.CustomerDataLoader
{
    public interface ICustomerDataLoaderService
    {
        Task<IEnumerable<Customer>> LoadCustomersAsync();

        Task SaveCustomerAsync(IEnumerable<Customer> customers);
    }
}
