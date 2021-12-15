using CustomerManagerApp.Backend.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository.Customer
{
    internal class MockCustomerRepository : ICustomerRepository
    {
        private List<CustomerValueObject> _customerList = new();
        private List<CustomerValueObject> _defaultList = new();

        public MockCustomerRepository(List<CustomerValueObject> DefaultList)
        {
            _customerList.AddRange(DefaultList);
            _defaultList = DefaultList;
        }
        public void DeleteStorageFile()
        {
            _customerList = null;
        }

        public Task<IEnumerable<CustomerValueObject>> LoadCustomersAsync()
        {
            if (_customerList is null)
            {
                _customerList = _defaultList;
            }
            IEnumerable<CustomerValueObject> list = _customerList;
            return Task.FromResult(list);
        }

        public Task SaveCustomerAsync(IEnumerable<CustomerValueObject> customers)
        {
            _customerList = customers.ToList();
            return Task.CompletedTask;
        }
    }
}
