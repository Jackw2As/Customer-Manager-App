using CustomerManagerApp.Backend.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository.Drink
{
    internal interface IDrinkLoaderService
    {
        public IEnumerable<DrinkValueObject> LoadDrinkTypes();

        public Task<IEnumerable<DrinkValueObject>> LoadDrinkTypesAsync();
    }
}
