using CustomerManagerApp.Backend.Entity;
using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.Services.CustomerDataLoader;
using CustomerManagerApp.Backend.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        //This Class should handle the creations ViewModels under the Main Window
        //This Class should pass data between the viewModels.
        //This Class should handle taking to any external classess.


        private CustomerViewModel selectedCustomer;

        private readonly CustomerDataContainer customerData;
        private readonly IDrinkLoaderService drinkData;

        public ObservableCollection<DrinkValueObject> DrinkTypes { get; } = new();

        public MainWindowViewModel(CustomerDataContainer customerData, IDrinkLoaderService drinkData)
        {
            this.customerData = customerData;
            this.drinkData = drinkData;
            this.DrinkTypes = new(drinkData.LoadDrinkTypes());

            ListViewModel = new(customerData, drinkData);
            ListViewModel.SelectedCustomerRaisedEvent += SelectedCustomerChangedEvent;
            ListViewModel.OnRefreshRaised += ListViewModel_OnRefreshRaised;

            EditViewModel = new(DrinkTypes);
            EditViewModel.RemoveSelectedCustomerEvent += EditViewModel_RemoveSelectedCustomerEvent;


        }

        private void EditViewModel_RemoveSelectedCustomerEvent(CustomerViewModel customer)
        {
            Load();
        }

        private void ListViewModel_OnRefreshRaised()
        {
            Load();
        }

        private void SelectedCustomerChangedEvent(CustomerViewModel customer)
        {
            SelectedCustomer = customer;
        }

        public CustomerViewModel SelectedCustomer
        {
            get { return selectedCustomer; }
            set {
                if (selectedCustomer != value)
                {
                    selectedCustomer = value;
                    EditViewModel.SelectedCustomer = value;
                    PropertyHasChanged();
                    PropertyHasChanged(nameof(IsCustomerSelected));
                }
            }
        }

        public ListViewModel ListViewModel { get; set; }

        public EditViewModel EditViewModel { get; set; }
        public bool IsCustomerSelected => SelectedCustomer != null;

        public bool IsLoading { get; private set; }  = false;

        public void SaveToStorage()
        {
            customerData.HardSave();
        }
        public void Load()
        {
            //stops multiple refreshes firing at once.
            if (IsLoading) return;
            IsLoading = true;

            var customers = customerData.Load();
            var drinkTypes = drinkData.LoadDrinkTypes();

            this.EditViewModel.DrinkTypes.Clear();
            this.ListViewModel.DrinkTypes.Clear();
            DrinkTypes?.Clear();

            this.ListViewModel.Customers.Clear();
            this.ListViewModel.FilterValue = string.Empty;

            this.EditViewModel.SelectedCustomer = null;
            this.ListViewModel.SelectedCustomer = null;

            foreach (var customer in customers)
            {
                var VM = new CustomerViewModel(customer, customerData);
                ListViewModel.Customers.Add(VM);
            }

            foreach (var drink in drinkTypes)
            {
                DrinkTypes.Add(drink);
                EditViewModel.DrinkTypes.Add(drink);
                ListViewModel.DrinkTypes.Add(drink);
            }
            


            IsLoading = false;
        }
    }
}
