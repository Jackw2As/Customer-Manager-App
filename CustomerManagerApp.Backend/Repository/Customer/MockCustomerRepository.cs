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

        public Task Create(CustomerEntity Model)
        {
            var values = _customerList.Find(item => item == Model);
            if(values != null)
            {
                throw new Exception($"Trying to Create a Model that already exists! use Update instead.");
            }
            _customerList.Add(Model);
            return Task.CompletedTask;
        }

        public Task Delete(CustomerEntity Model)
        {
            var sucessfulDelete = _customerList.Remove(Model);
            if(!sucessfulDelete)
            {
                throw new Exception($"Trying to Delete a Model that does not exist. Perhaps it was already removed!");
            }
            return Task.CompletedTask;
        }

        public Task DeleteAll()
        {
            _customerList.Clear();
            return Task.CompletedTask;
        }

        public Task<CustomerEntity> Read(string id)
        {
            var customer = _customerList.Find(customer => customer.ID == id);
            if(customer == null)
            {
                throw new ArgumentNullException($"{nameof(id)} was not a valid ID!");
            }
            return Task.FromResult(customer);
        }

        public Task<List<CustomerEntity>> ReadAll()
        {
            return Task.FromResult(_customerList);
        }
        public Task Update(CustomerEntity Model)
        {
            var success = _customerList.Remove(Model);
            if (!success)
            {
                throw new ArgumentNullException(nameof(Model), $"The Model did not exist in the database. Call Create instead!");
            }
            
            _customerList.Add(Model);
            return Task.CompletedTask;
        }
    }
}
