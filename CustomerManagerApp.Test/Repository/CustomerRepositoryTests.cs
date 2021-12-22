using Xunit;
using System.Collections.Generic;
using CustomerManagerApp.Backend.Repository.Customer;
using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.Entities;

namespace CustomerManagerApp.Test
{
    public class CustomerRepositoryTests
    {
        public CustomerRepositoryTests()
        {
            var List = new MockDrinkRepository().LoadDrinkTypes() as List<DrinkEntity>;
            if (List != null) drinksTypes = List;
            else drinksTypes = new();

            mockDefaultCustomersList = new List<CustomerEntity>
            {
                new("John", "1", drinksTypes[0].Id),
                new("John", "2", drinksTypes[0].Id),
                new("Susan", "1", drinksTypes[0].Id, true),
                new("Susan", "2", drinksTypes[0].Id)
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
            Loader.DeleteStorageFile();
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

            await Loader.SaveCustomerAsync(new List<CustomerEntity>());
            var ConfirmEmpty = await Loader.LoadCustomersAsync();
            Assert.Empty(ConfirmEmpty);

            Loader.DeleteStorageFile();
            var customers = await Loader.LoadCustomersAsync();
            
            Assert.NotEmpty(customers);
        }

        [Fact(DisplayName = "Add Customer")]
        /// Tests:
        /// Adding a new Customer
        public async void AddNewCustomer()
        {
            var Loader = CreateMock();

            IEnumerable<CustomerEntity> collection = await Loader.LoadCustomersAsync();
            if (collection is null)
            {
                Assert.NotNull(collection);
                return;
            }

            Assert.NotEmpty(collection);

            List<CustomerEntity> collection2 = new();
            collection2.AddRange(collection);

            CustomerEntity customer = new("Zack", "0", drinksTypes[0].Id, true);
            collection2.Add(customer);
            await Loader.SaveCustomerAsync(collection2);

            var customers = await Loader.LoadCustomersAsync();
            Assert.Contains(customer, customers);
        }

        [Fact(DisplayName = "Remove Customer")]
        /// Tests:
        /// Removing a new Customer
        public async void RemoveNewCustomer()
        {
            var Loader = CreateMock();

            List<CustomerEntity>? collection = await Loader.LoadCustomersAsync() as List<CustomerEntity>;
            if (collection is null)
            {
                Assert.NotNull(collection);
                return;
            }

            Assert.NotEmpty(collection);
            
            CustomerEntity customer = (CustomerEntity)collection[0];
            collection.Remove(customer);
            await Loader.SaveCustomerAsync(collection);

            var customers = await Loader.LoadCustomersAsync();
            Assert.DoesNotContain(customer, customers);
        }

    }
}