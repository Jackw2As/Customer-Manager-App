using CustomerManagerApp.Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Services.DrinkRoleLoader
{
    public class MockDrinkTypesLoader : IDrinkLoaderService
    {
        public IEnumerable<Drink> LoadDrinkTypes()
        {
            return CreateDrinksList();
        }

        public Task<IEnumerable<Drink>> LoadDrinkTypesAsync()
        {
            var jobs = CreateDrinksList();
            return Task<IEnumerable<Drink>>.Factory.StartNew(() => jobs);
        }

        private IEnumerable<Drink> CreateDrinksList()
        {
            return new List<Drink>() {
            new("English Breakfast"),
            new("Hot Chocolate"),
            new("Coffee")
            };
        }
    }
}
