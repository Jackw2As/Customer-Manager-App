using CustomerManagerApp.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository.Drink
{
    public interface IDrinkRepository
    {
        public IEnumerable<DrinkEntity> LoadDrinkTypes();

        public Task<IEnumerable<DrinkEntity>> LoadDrinkTypesAsync();
    }
}
