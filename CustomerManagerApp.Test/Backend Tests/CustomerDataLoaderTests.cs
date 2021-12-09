using CustomerManagerApp.Backend.Services.CustomerDataLoader;
using Xunit;
using CustomerManagerApp.Backend.Services;
using CustomerManagerApp.Backend.Model;
using System.Collections.Generic;
using CustomerManagerApp.Backend.Services.DrinkRoleLoader;

namespace CustomerManagerApp.Test
{
    public class CustomerDataLoaderTests
    {
        public CustomerDataLoaderTests()
        {
            var List = new MockDrinkTypesLoader().LoadDrinkTypes() as List<Drink>;
            if (List != null) drinksTypes = List;
            else drinksTypes = new();

            mockDefaultCustomersList = new List<Customer>
            {
                new("John", "1", drinksTypes[0].Id),
                new("John", "2", drinksTypes[0].Id),
                new("Susan", "1", drinksTypes[0].Id, true),
                new("Susan", "2", drinksTypes[0].Id)
            };

            
        }

        private List<Drink> drinksTypes
        {
            get; init;
        }

        private List<Customer> mockDefaultCustomersList;


        private ICustomerDataLoaderService CreateMock()
        {
            //clear old test file
            var Loader = new CustomerDataJsonLoader("Tests");
            Loader.DeleteStorageFile();
            //create new test file
            return new CustomerDataJsonLoader("Tests", mockDefaultCustomersList);

            
        }


        
        [Fact(DisplayName ="Delete File")]
        /// Tests:
        /// Delete Function.
        /// SaveCustomerAsync method works with a new list.
        /// Tests to see that if no File exists a new file is created with default values.
        public async void DeleteFile()
        {
            var Loader = CreateMock();

            await Loader.SaveCustomerAsync(new List<Customer>());
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

            IEnumerable<Customer> collection = await Loader.LoadCustomersAsync();
            if (collection is null)
            {
                Assert.NotNull(collection);
                return;
            }

            Assert.NotEmpty(collection);

            List<Customer> collection2 = new();
            collection2.AddRange(collection);

            Customer customer = new("Zack", "0", drinksTypes[0].Id, true);
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

            List<Customer>? collection = await Loader.LoadCustomersAsync() as List<Customer>;
            if (collection is null)
            {
                Assert.NotNull(collection);
                return;
            }

            Assert.NotEmpty(collection);
            
            Customer customer = (Customer)collection[0];
            collection.Remove(customer);
            await Loader.SaveCustomerAsync(collection);

            var customers = await Loader.LoadCustomersAsync();
            Assert.DoesNotContain(customer, customers);
        }

    }
}