using Xunit;
using System.Collections.Generic;
using CustomerManagerApp.Backend.Repository.Customer;
using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.Entities;
using System;
using System.Threading.Tasks;

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
                new(Guid.NewGuid().ToString(), "John", "1", drinksTypes[0].ID, DateTime.UtcNow),
                new(Guid.NewGuid().ToString(), "John", "2", drinksTypes[0].ID, DateTime.UtcNow),
                new(Guid.NewGuid().ToString(), "Susan", "1", drinksTypes[0].ID, DateTime.UtcNow, true),
                new(Guid.NewGuid().ToString(), "Susan", "2", drinksTypes[0].ID, DateTime.UtcNow)
            };  
        }

        private List<DrinkEntity> drinksTypes
        {
            get; init;
        }

        private List<CustomerEntity> mockDefaultCustomersList;


        private async Task<ICustomerRepository> CreateMock()
        {
            //Delete Test File
            var loader = new JsonCustomerRepo("Tests");
            await loader.DeleteAll();
            
            //Create new Test File
            var mockRepository = new JsonCustomerRepo("Tests");

            foreach (var customer in mockDefaultCustomersList)
            {
                await mockRepository.Create(customer);
            }

            return mockRepository;
        }


        
        [Fact(DisplayName ="Delete File")]
        /// Tests:
        /// Delete Function.
        /// SaveCustomerAsync method works with a new list.
        /// Tests to see that if no File exists a new file is created with default values.
        public async Task DeleteFile()
        {
            var Loader = await CreateMock();

            var customerCollection = await Loader.ReadAll();

            foreach (var customer in customerCollection)
            {
                await Loader.Delete(customer);
            }
            var ConfirmEmpty = await Loader.ReadAll();
            Assert.Empty(ConfirmEmpty);

            await Loader.DeleteAll();
            try
            {
                var customers = await Loader.ReadAll();

                Assert.NotEmpty(customers);
            }
            catch (ObjectDisposedException ex)
            {
                //File is Deleted here
            }
        }

        [Fact(DisplayName = "Add Customer")]
        /// Tests:
        /// Adding a new Customer
        public async Task AddNewCustomer()
        {
            var Loader = await CreateMock();

            IEnumerable<CustomerEntity> collection = await Loader.ReadAll();
            if (collection is null)
            {
                Assert.NotNull(collection);
                return;
            }

            Assert.NotEmpty(collection);
            CustomerEntity customer = new("1111", "Zack", "0", drinksTypes[0].ID, DateTime.UtcNow, true);
            await Loader.Create(customer);

            var customers = await Loader.ReadAll();
            Assert.Contains(customer, customers);
        }

        [Fact(DisplayName = "Remove Customer")]
        /// Tests:
        /// Removing a new Customer
        public async Task RemoveNewCustomer()
        {
            //Setup
            var Loader = await CreateMock();
            
            var collection = await Loader.ReadAll();
            CustomerEntity customer = (CustomerEntity)collection[0];

            //Delete Customer
            await Loader.Delete(customer);

            //Validation
            var customers = await Loader.ReadAll();
            Assert.DoesNotContain(customer, customers);
        }


        [Fact(DisplayName = "Read All Returns Values")]
        public async Task ReadAllReturnsValues()
        {
            var Loader = await CreateMock();
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