using CustomerManagerApp.Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Services.CustomerDataLoader
{
    public class MockCustomerDataLoaderService : ICustomerDataLoaderService
    {
        private List<Customer> _customerList = new();
        private List<Customer> _defaultList = new();

        public MockCustomerDataLoaderService(List<Customer>DefaultList)
        {
            _customerList.AddRange(DefaultList);
            _defaultList = DefaultList;
        }
        public void DeleteStorageFile()
        {
            _customerList = null;
        }

        public Task<IEnumerable<Customer>> LoadCustomersAsync()
        {
            if(_customerList is null)
            {
                _customerList = _defaultList;
            }
            IEnumerable<Customer> list = _customerList;
            return Task.FromResult(list);
        }

        public Task SaveCustomerAsync(IEnumerable<Customer> customers)
        {
            _customerList = customers.ToList();
            return Task.CompletedTask;
        }
    }
}
