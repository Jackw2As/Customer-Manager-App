using CustomerManagerApp.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository.Drink
{
    public class MockDrinkRepository : IDrinkRepository
    {
        public IEnumerable<DrinkEntity> LoadDrinkTypes()
        {
            return CreateDrinksList();
        }

        public Task<IEnumerable<DrinkEntity>> LoadDrinkTypesAsync()
        {
            var drinks = CreateDrinksList();
            return Task<IEnumerable<DrinkEntity>>.Factory.StartNew(() => drinks);
        }

        private IEnumerable<DrinkEntity> CreateDrinksList()
        {
            return new List<DrinkEntity>() {
            new("English Breakfast"),
            new("Hot Chocolate"),
            new("Coffee")
            };
        }
    }
}
