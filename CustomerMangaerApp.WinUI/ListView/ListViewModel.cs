using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Backend.Repository.Customer;
using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.Backend.Service.FilterService;
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
        public string FilterValue = "";
        private DataService Data { get; }

        public event RefreshEvent OnRefreshRaised;
        public void RefreshList()
        {
            OnRefreshRaised?.Invoke();
            this.PropertyHasChanged(nameof(FilteredList));
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

        public ListViewModel(ref DataService DataService)
        {
            Data = DataService;
        }
        public async void CustomerAdd()
        {
            var drinks = await Data.GetDrinksAsync();
            var drink = drinks.First().ID;
            var customer = new CustomerEntity(Guid.NewGuid().ToString(), "new customer", "", drink);
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

        internal void Load()
        {
            RefreshDatabaseList();
            Filter();
        }

        public void Filter()
        {
            var filterService = new FilterService();
            FilteredList.Clear();

            List<CustomerEntity> databaseCustomerList = new();
            //Converting CustomerWrapper into CustomerEntity
            Parallel.ForEach(DatabaseCustomerList, customerWrapper => databaseCustomerList.Add(customerWrapper.GetWrappedCustomer));

            var filteredList = filterService.FilterCustomerList(databaseCustomerList, FilterValue);

            Parallel.ForEach(filteredList, customerEntity => FilteredList.Add( new(customerEntity) ));
        }

        private void RefreshDatabaseList()
        {
            Data.Refresh();
            DatabaseCustomerList.Clear();
            var data = Data.GetCustomerList();
            foreach (var customer in data)
            {
                DatabaseCustomerList.Add(new CustomerWrapper(customer));
            }
        }

        
    }
}
