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
    public class MainWindowViewModel : ViewModelBase
    {
        private CustomerViewModel selectedCustomer;
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

        public CustomerViewModel SelectedCustomer
        {
            get { return selectedCustomer; }
            set {
                if (selectedCustomer != value)
                {
                    selectedCustomer = value;
                    PropertyHasChanged();
                    PropertyHasChanged(nameof(IsCustomerSelected));
                }
            }
        }

        public bool IsCustomerSelected => SelectedCustomer != null;

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

        public void AddCustomer()
        {
            Customer customer = new("new customer", "", DrinkTypes[0].Id);
            var defualtCustomer = new CustomerViewModel(customer, customerData);
            Customers.Add(defualtCustomer);
        }

        public void RemoveSelectedCustomer()
        {
            SelectedCustomer.Remove();
            Customers.Remove(SelectedCustomer);
            SelectedCustomer = null;
            
        }

        public void SaveToStorage()
        {
            customerData.HardSave();
        }

        public string FilterValue = "";
        public void Filter()
        {
            foreach (var customer in Customers)
            {
                customer.SaveCustomer();
            }
            var customers = customerData.Load();
            
            Customers.Clear();
            filteredList.Clear();
            Parallel.ForEach(customers, customer => FilterByName(FilterValue, customer));

            foreach (var item in filteredList)
            {
                Customers.Add(item);
            }
            
        }
        private List<CustomerViewModel> filteredList = new();
        private void FilterByName(string FilterText, Customer customer)
        {
            if (customer.FirstName.Contains(FilterText) == false && customer.LastName.Contains(FilterText) == false)
            {
                return;
            }

            var model = new CustomerViewModel(customer, customerData);
            if(!filteredList.Contains(model))
            {
                filteredList.Add(model);
            }
        }
        
    }
}
