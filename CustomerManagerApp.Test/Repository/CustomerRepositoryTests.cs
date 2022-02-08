using Xunit;
using System.Collections.Generic;
using CustomerManagerApp.Backend.Repository.Customer;
using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.Entities;
using System;

namespace CustomerManagerApp.Test
{
    public class CustomerRepositoryTests
    {
        public CustomerRepositoryTests()
        {
            var List = new MockDrinkRepository().ReadAll().Result as List<DrinkEntity>;
            if (List != null) drinksTypes = List;
            else drinksTypes = new();

            mockDefaultCustomersList = new List<CustomerEntity>
            {
                new(Guid.NewGuid().ToString(), "John", "1", drinksTypes[0].ID),
                new(Guid.NewGuid().ToString(), "John", "2", drinksTypes[0].ID),
                new(Guid.NewGuid().ToString(), "Susan", "1", drinksTypes[0].ID, true),
                new(Guid.NewGuid().ToString(), "Susan", "2", drinksTypes[0].ID)
            };  
        }

        private List<DrinkEntity> drinksTypes
        {
            get; init;
        }

        private List<CustomerEntity> mockDefaultCustomersList;


        private ICustomerRepository CreateMock()
        {
            //clear old test file
            var Loader = new JsonCustomerRepository("Tests");
            Loader.DeleteAll();
            //create new test file
            return new JsonCustomerRepository("Tests", mockDefaultCustomersList);
        }


        
        [Fact(DisplayName ="Delete File")]
        /// Tests:
        /// Delete Function.
        /// SaveCustomerAsync method works with a new list.
        /// Tests to see that if no File exists a new file is created with default values.
        public async void DeleteFile()
        {
            var Loader = CreateMock();

            var customerCollection = await Loader.ReadAll();

            foreach (var customer in customerCollection)
            {
                Loader.Delete(customer);
            }
            var ConfirmEmpty = await Loader.ReadAll();
            Assert.Empty(ConfirmEmpty);

            Loader.DeleteAll();
            var customers = await Loader.ReadAll();
            
            Assert.NotEmpty(customers);
        }

        [Fact(DisplayName = "Add Customer")]
        /// Tests:
        /// Adding a new Customer
        public async void AddNewCustomer()
        {
            var Loader = CreateMock();

            IEnumerable<CustomerEntity> collection = await Loader.ReadAll();
            if (collection is null)
            {
                Assert.NotNull(collection);
                return;
            }

            Assert.NotEmpty(collection);
            CustomerEntity customer = new("1111", "Zack", "0", drinksTypes[0].ID, true);
            Loader.Create(customer);

            var customers = await Loader.ReadAll();
            Assert.Contains(customer, customers);
        }

        [Fact(DisplayName = "Remove Customer")]
        /// Tests:
        /// Removing a new Customer
        public async void RemoveNewCustomer()
        {
            //Setup
            var Loader = CreateMock();
            
            var collection = await Loader.ReadAll();
            CustomerEntity customer = (CustomerEntity)collection[0];

            //Delete Customer
            Loader.Delete(customer);

            //Validation
            var customers = await Loader.ReadAll();
            Assert.DoesNotContain(customer, customers);
        }


        [Fact(DisplayName = "Read All Returns Values")]
        public async void ReadAllReturnsValues()
        {
            var Loader = CreateMock();
            List<CustomerEntity>? collection = await Loader.ReadAll();
            if (collection is null)
            {
                Assert.NotNull(collection);
                Assert.NotEmpty(collection);
                return;
            }
        }

    }
}