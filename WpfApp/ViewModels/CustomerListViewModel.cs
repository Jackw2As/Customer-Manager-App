using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.WpfApp.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.WpfApp.ViewModels
{
    internal delegate void RefreshEvent();
    internal delegate void CustomerSelectionChangedEvent(CustomerWrapper? customer);
    public class CustomerListViewModel : ViewModelBase
    {
        //fields
        private DataService? Data { get; set; }
        private DrinkWrapper? DefaultDrink { get; set; }
        private bool isLoading = false;
        private CustomerWrapper? selectedCustomer = null;

        //contructors
        public CustomerListViewModel()
        {
            setupModel();
        }

        //properties
        public ObservableCollection<CustomerWrapper> FilteredCustomerList { get; set; } = new();
        public ObservableCollection<CustomerWrapper> UnFilteredCustomerList { get; } = new();
        public CustomerWrapper? SelectedCustomer
        {
            get => selectedCustomer;
            set
            {
                if (selectedCustomer != value)
                {
                    selectedCustomer = value;
                    SelectedCustomerChanged?.Invoke(selectedCustomer);
                    PropertyHasChanged();
                }
            }
        }
        public string FilterValue { get; set; } = "";

        //events
        /// <summary>
        /// Called when a new customer is selected
        /// </summary>
        internal event CustomerSelectionChangedEvent? SelectedCustomerChanged;
        internal event RefreshEvent? OnRefresh;
        //methods
        public void Load()
        {
            if (Data == null && isLoading) return;
            isLoading = true;

            var Customerscollection = Data!.GetCustomerList();
            UnFilteredCustomerList.Clear();
            FilteredCustomerList.Clear();

            foreach (var customer in Customerscollection)
            {
                if (customer != null)
                {
                    UnFilteredCustomerList.Add(new(customer));
                    FilteredCustomerList.Add(new(customer));
                }
            }
            PropertyHasChanged(nameof(UnFilteredCustomerList));
            PropertyHasChanged(nameof(FilteredCustomerList));
            isLoading = false;
        }
        public void RefreshList()
        {
            if (OnRefresh != null)
            {
                OnRefresh?.Invoke();
            }
            else
            {
                Load();
            }
        }
        public async void AddNewCustomerToList()
        {
            if(DefaultDrink == null)
            {
                if(Data == null) return;

                var drinks = await Data.GetDrinksAsync();
                var drink = drinks.FirstOrDefault()!;
                DefaultDrink = new(drink);
            }
            CustomerWrapper defualtCustomer = new(DefaultDrink);
            UnFilteredCustomerList.Add(defualtCustomer);
        }
        public void Filter()
        {
            FilteredCustomerList = UnFilteredCustomerList;
            Parallel.ForEach(FilteredCustomerList, customer => FilterByName(FilterValue, customer));
        }

        //private methods
        private async void setupModel()
        {
            Data = await DataService.CreateDataServiceObjectAsync();
            var list = await Data.GetDrinksAsync();
            DefaultDrink = new(list.First());
        }
        private void FilterByName(string FilterText, CustomerWrapper customer)
        {
            if (customer.FirstName.Contains(FilterText) && customer.LastName.Contains(FilterText))
            {
                FilteredCustomerList.Add(customer);
            }
        }
    }
}
