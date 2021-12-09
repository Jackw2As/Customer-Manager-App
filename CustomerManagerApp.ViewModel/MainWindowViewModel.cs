using CustomerManagerApp.Backend.Entity;
using CustomerManagerApp.Backend.Model;
using CustomerManagerApp.Backend.Services.CustomerDataLoader;
using CustomerManagerApp.Backend.Services.DrinkRoleLoader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private CustomerViewModel selectedEmployee;
        private readonly IDrinkLoaderService drinkData;

        private readonly CustomerDataContainer customerData;

        public MainWindowViewModel(ICustomerDataLoaderService customerData, 
            IDrinkLoaderService drinkData
            )
        {
            this.drinkData = drinkData;
            this.customerData = new(customerData);
        }
        public ObservableCollection<CustomerViewModel> Customers { get; } = new();
        public ObservableCollection<Drink> DrinkTypes { get; } = new();
        public CustomerViewModel SelectedEmployee
        {
            get { return selectedEmployee; }
            set {
                if (selectedEmployee != value)
                {
                    selectedEmployee = value;
                    PropertyHasChanged();
                    PropertyHasChanged(nameof(IsEmployeeeSeelected));
                }
            }
        }

        public bool IsEmployeeeSeelected => SelectedEmployee != null;

        public bool IsLoading { get; private set; }  = false;

        public async void  Load()
        {
            //stops multiple refreshes firing at once.
            if (IsLoading) return;

            IsLoading = true;
            var customers = customerData.Load();
            var drinkTypes = await drinkData.LoadDrinkTypesAsync();

            Customers.Clear();
            DrinkTypes.Clear();

            foreach (var customer in customers)
            {
                var VM = new CustomerViewModel(customer, customerData);
                Customers.Add(VM);
            }

            foreach (var drink in drinkTypes)
            {
                DrinkTypes.Add(drink);
            }
            IsLoading = false;
        }

    }
}
