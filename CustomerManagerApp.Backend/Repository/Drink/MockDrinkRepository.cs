using CustomerManagerApp.Backend.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository.Drink
{
    internal class MockDrinkRepository : IDrinkRepository
    {
        public IEnumerable<DrinkValueObject> LoadDrinkTypes()
        {
            return CreateDrinksList();
        }

        public Task<IEnumerable<DrinkValueObject>> LoadDrinkTypesAsync()
        {
            var drinks = CreateDrinksList();
            return Task<IEnumerable<DrinkValueObject>>.Factory.StartNew(() => drinks);
        }

        private IEnumerable<DrinkValueObject> CreateDrinksList()
        {
            return new List<DrinkValueObject>() {
            new("English Breakfast"),
            new("Hot Chocolate"),
            new("Coffee")
            };
        }
    }
}
