using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.WinUI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.ViewModel
{
    public delegate void RefreshEvent();
    public delegate void CustomerChangedEvent(CustomerWrapper customer);
    public class ListViewModel : ViewModelBase
    {
        public ObservableCollection<CustomerWrapper> Customers { get; } = new();
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
                    SelectedCustomerRaisedEvent.Invoke(selectedCustomer);
                    PropertyHasChanged();
                }
            }
        }

        public ListViewModel(DataService DataService)
        {
            Data = DataService;
        }
        public void CustomerAdd()
        {
            var customer = new CustomerEntity("new customer", "", Data.GetDrinksAsync().Result.First().Id);
            var defualtCustomer = new CustomerWrapper(customer);
            Customers.Add(defualtCustomer);
        }

        public void CustomerRemove(CustomerWrapper customer)
        {
            Customers.Remove(customer);
        }

        public ObservableCollection<CustomerWrapper> CustomerList()
        {
            return Customers;
        }

        public string FilterValue = "";
        public async void Filter()
        {
            foreach (var customer in Customers)
            {
            }
            var customers = await Data.GetCustomersAsync();

            Customers.Clear();
            filteredList.Clear();
            Parallel.ForEach(customers, customer => FilterByName(FilterValue, customer));

            foreach (var item in filteredList)
            {
                Customers.Add(item);
            }

        }

        internal async void Load()
        {
            Customers.Clear();
            var data = await Data.GetCustomersAsync();
            foreach (var customer in data)
            {
                Customers.Add(new CustomerWrapper(customer));
            }
        }

        private List<CustomerWrapper> filteredList = new();



        private void FilterByName(string FilterText, CustomerEntity customer)
        {
            if (customer.FirstName.Contains(FilterText) == false && customer.LastName.Contains(FilterText) == false)
            {
                return;
            }

            var drink = new DrinkWrapper(Data.GetDrinks().First());
            var model = new CustomerWrapper(drink);
            if (!filteredList.Contains(model))
            {
                filteredList.Add(model);
            }
        }
    }
}
