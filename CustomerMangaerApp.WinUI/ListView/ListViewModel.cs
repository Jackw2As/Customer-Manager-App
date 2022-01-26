using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.WinUI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.ViewModel
{
    public delegate void RefreshEvent();
    public delegate void CustomerChangedEvent(CustomerWrapper customer);
    public class ListViewModel : ViewModelBase
    {
        private List<CustomerWrapper> DatabaseCustomerList { get; } = new();

        public ObservableCollection<CustomerWrapper> FilteredList { get; } = new();
        private DataService Data { get; }

        public event RefreshEvent OnRefreshRaised;
        public void RefreshList()
        {
            OnRefreshRaised?.Invoke();
        }

        /// <summary>
        /// Called when a new customer is selected
        /// </summary>
        public event CustomerChangedEvent SelectedCustomerRaisedEvent;

        private CustomerWrapper selectedCustomer;
        public CustomerWrapper SelectedCustomer
        {
            get => selectedCustomer; 
            set
            {
                if(selectedCustomer != value)
                {
                    selectedCustomer = value;
                    PropertyHasChanged();
                    SelectedCustomerRaisedEvent?.Invoke(SelectedCustomer);
                }
            }
        }

        public ListViewModel(DataService DataService)
        {
            Data = DataService;
        }
        public async void CustomerAdd()
        {
            var drinks = await Data.GetDrinksAsync();
            var drink = drinks.First().Id;
            var customer = new CustomerEntity("new customer", "", drink);
            var defualtCustomer = new CustomerWrapper(customer);
            DatabaseCustomerList.Add(defualtCustomer);
            FilteredList.Add(defualtCustomer);
        }

        public void CustomerRemove(CustomerWrapper customer)
        {
            DatabaseCustomerList.Remove(customer);
            FilteredList.Remove(customer);
        }

        public List<CustomerWrapper> GetDatabaseCustomerList()
        {
            return new(DatabaseCustomerList);
        }

        

        internal async void Load()
        {
            await RefreshDatabaseList();

            Filter();
        }

        private async Task RefreshDatabaseList()
        {
            DatabaseCustomerList.Clear();
            var data = await Data.GetCustomersAsync();
            foreach (var customer in data)
            {
                DatabaseCustomerList.Add(new CustomerWrapper(customer));
            }
        }

        public string FilterValue = "";
        private List<CustomerWrapper> filteredList = new();
        public void Filter()
        {
            filteredList.Clear();
            FilteredList.Clear();

            //If Empty add Database List
            if (FilterValue == String.Empty)
            { 
                foreach (var customer in DatabaseCustomerList)
                {
                    FilteredList.Add(customer);
                }
                return;
            }

            Parallel.ForEach(DatabaseCustomerList, customer => FilterByName(FilterValue, customer));

            foreach (var customer in filteredList)
            {
                FilteredList.Add(customer);
            }
        }

        private void FilterByName(string FilterText, CustomerWrapper customer)
        {
            if (customer.FirstName.Contains(FilterText) == false && customer.LastName.Contains(FilterText) == false)
            {
                return;
            }
            filteredList.Add(customer);
        }
    }
}
