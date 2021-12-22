using CustomerManagerApp.Backend.ValueObjects;
using CustomerMangaerApp.WinUI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.WinUI.Wrapper
{
    public class DrinkWrapper : BaseWrapper
    {
        private DrinkValueObject drink;
        public DrinkWrapper()
        {
            drink = new("");
        }

        public DrinkWrapper(DrinkValueObject Drink)
        {
            drink = Drink;
        }

        public string Id
        {
            get => drink.Id; 
        }
        public string Name
        {
            get => drink.Name; 
            set
            {
                if (drink.Name != value)
                {
                    drink.Name = value;
                    propertyChanged();
                }
            }
        }
    }
}
