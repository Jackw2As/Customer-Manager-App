using CustomerManagerApp.Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Services.DrinkRoleLoader
{
    public interface IDrinkLoaderService
    {
        public IEnumerable<Drink> LoadDrinkTypes();

        public Task<IEnumerable<Drink>> LoadDrinkTypesAsync();
    }
}
