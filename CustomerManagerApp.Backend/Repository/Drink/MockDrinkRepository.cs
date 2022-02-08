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
        public DrinkEntity Read(string id)
        {
            var drinks = CreateDrinksList();

            var drink = drinks.Find(_ => _.ID == id);

            if (drink == null)
            {
                throw new ArgumentNullException($"{nameof(id)} ID was not found in the List");
            }

            return drink;
        }

        public List<DrinkEntity> ReadAll() => CreateDrinksList();

        public void Create(DrinkEntity Model)
        {
            string message = $"The Drink Mock Repository Class doesn't support {nameof(Create)} method!";
            throw new NotSupportedException(message);
        }

        public void Delete(DrinkEntity Model)
        {
            string message = $"The Drink Mock Repository Class doesn't support {nameof(Delete)} method!";
            throw new NotSupportedException(message);
        }

        public void DeleteAll()
        {
            string message = $"The Drink Mock Repository Class doesn't support {nameof(DeleteAll)} method!";
            throw new NotSupportedException(message);
        }
        
        public void Update(DrinkEntity Model)
        {
            string message = $"The Drink Mock Repository Class doesn't support {nameof(Update)} method!";
            throw new NotSupportedException(message);
        }

        private List<DrinkEntity> CreateDrinksList()
        {
            return new List<DrinkEntity>() {
            new("English Breakfast"),
            new("Hot Chocolate"),
            new("Coffee")
            };
        }
    }
}
