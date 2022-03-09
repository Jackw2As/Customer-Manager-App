using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Backend.Repository.Drink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository.Customer
{
    public class MockCustomerRepository : ICustomerRepository
    {
        private List<CustomerEntity>? _customerList;
        private IDrinkRepository? drinkRepository;

        public MockCustomerRepository(List<CustomerEntity> defaultList)
        {
            _customerList = defaultList;
        }

        public MockCustomerRepository(IDrinkRepository repository)
        {
            drinkRepository = repository;
        }

        private async Task createCustomerList()
        {
            if (drinkRepository == null)
            {
                throw new Exception("This shouldn't be possible! Somehow MockCustomer Repository " +
                    "was created without either creating a _customerList or drinkRepository!");
            }

            var drinks = await drinkRepository.ReadAll();
            var drinkId = drinks.First().ID;
            if(drinkId == null) throw new ArgumentNullException(nameof(drinkId));

            _customerList = new()
            {
                new CustomerEntity(Guid.NewGuid().ToString(), "Jack", "Yellow", drinkId, new DateTime(2020, 09, 05)),
                new CustomerEntity(Guid.NewGuid().ToString(), "John", "Blue", drinkId, new DateTime(2011, 09, 05)),
                new CustomerEntity(Guid.NewGuid().ToString(), "Sarah", "Purple", drinkId, new DateTime(2017, 08, 06))
            };
        }

        public async Task Create(CustomerEntity Model)
        {
            if(_customerList == null)
            {
                await createCustomerList();
            }

            var values = _customerList!.Find(item => item == Model);
            if(values != null)
            {
                throw new Exception($"Trying to Create a Model that already exists! use Update instead.");
            }
            _customerList.Add(Model);
        }

        public async Task Delete(CustomerEntity Model)
        {
            if (_customerList == null)
            {
                await createCustomerList();
            }

            var sucessfulDelete = _customerList!.Remove(Model);
            if(!sucessfulDelete)
            {
                throw new Exception($"Trying to Delete a Model that does not exist. Perhaps it was already removed!");
            }
        }

        public async Task DeleteAll()
        {
            if (_customerList == null)
            {
                await createCustomerList();
            }
            _customerList!.Clear();
        }

        public async Task<CustomerEntity> Read(string id)
        {
            if (_customerList == null)
            {
                await createCustomerList();
            }

            var customer = _customerList!.Find(customer => customer.ID == id);
            if(customer == null)
            {
                throw new ArgumentNullException($"{nameof(id)} was not a valid ID!");
            }
            return customer;
        }

        public async Task<List<CustomerEntity>> ReadAll()
        {
            if (_customerList == null)
            {
                await createCustomerList();
            }
            return _customerList!;
        }
        public async Task Update(CustomerEntity Model)
        {
            if (_customerList == null)
            {
                await createCustomerList();
            }

            var success = _customerList!.Remove(Model);
            if (!success)
            {
                throw new ArgumentNullException(nameof(Model), $"The Model did not exist in the database. Call Create instead!");
            }
            
            _customerList.Add(Model);
        }
    }
}
