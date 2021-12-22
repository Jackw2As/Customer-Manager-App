using CustomerManagerApp.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository.Customer
{
    public class MockCustomerRepository : ICustomerRepository
    {
        private List<CustomerEntity> _customerList = new();
        private List<CustomerEntity> _defaultList = new();

        public MockCustomerRepository(List<CustomerEntity> DefaultList)
        {
            _customerList.AddRange(DefaultList);
            _defaultList = DefaultList;
        }
        public void DeleteStorageFile()
        {
            _customerList = null;
        }

        public Task<IEnumerable<CustomerEntity>> LoadCustomersAsync()
        {
            if (_customerList is null)
            {
                _customerList = _defaultList;
            }
            IEnumerable<CustomerEntity> list = _customerList;
            return Task.FromResult(list);
        }

        public Task SaveCustomerAsync(IEnumerable<CustomerEntity> customers)
        {
            _customerList = customers.ToList();
            return Task.CompletedTask;
        }
    }
}
