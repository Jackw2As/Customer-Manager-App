using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.Wpf.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Wpf.CustomerList
{
    internal delegate void RefreshEvent();
    internal delegate void CustomerSelectionChangedEvent(CustomerWrapper? customer);
    internal class CustomerListViewModel : ViewModelBase
    {
        internal ObservableCollection<CustomerWrapper> FilteredCustomerList { get; private set; } = new();
        internal ObservableCollection<CustomerWrapper> UnFilteredCustomerList { get; } = new();
        internal DrinkWrapper DefaultDrink { get; }
        protected DataService Data { get; }

        internal CustomerListViewModel(ref DataService DataService, DrinkWrapper defaultDrink)
        {
            Data = DataService;
            DefaultDrink = defaultDrink;
        }

        public CustomerListViewModel()
        {
            Data = new();
            DefaultDrink = new(Data.GetDrinksAsync().Result?.FirstOrDefault() ?? new("default drink value not set!"));
        }

        //The List View is responsible for populating the list with new values.
        private bool isLoading = false;
        public async void Load()
        {
            if (isLoading) return;
            isLoading = true;

            var Customerscollection = await Data.GetCustomersAsync();
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
            isLoading = false;
        }

        //The List View is responsible for raising an event which tells other views which item in the list is selected.

        /// <summary>
        /// Called when a new customer is selected
        /// </summary>
        internal event CustomerSelectionChangedEvent? SelectedCustomerChanged;
        
        private CustomerWrapper? selectedCustomer = null;
        internal CustomerWrapper? SelectedCustomer
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


        //The List View is responsible for implementing any Commands required by the View. 
        internal event RefreshEvent? OnRefresh;
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

        public void AddNewCustomerToList()
        {
            CustomerWrapper defualtCustomer = new(DefaultDrink);
            UnFilteredCustomerList.Add(defualtCustomer);
        }

        public string FilterValue = "";
        public void Filter()
        {
            FilteredCustomerList = UnFilteredCustomerList;
            Parallel.ForEach(FilteredCustomerList, customer => FilterByName(FilterValue, customer));
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
