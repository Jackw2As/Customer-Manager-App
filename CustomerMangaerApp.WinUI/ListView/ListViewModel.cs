using CustomerManagerApp.Backend.Repository.Drink;
using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.Backend.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.ViewModel
{
    public delegate void RefreshEvent();
    public delegate void CustomerChangedEvent(CustomerViewModel customer);
    public class ListViewModel : ViewModelBase
    {
        public ObservableCollection<CustomerViewModel> Customers { get; } = new();
        public ObservableCollection<DrinkValueObject> DrinkTypes { get; } = new();
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
        public CustomerViewModel SelectedCustomer
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
            CustomerValueObject customer = new("new customer", "", DrinkTypes[0].Id);
            var defualtCustomer = new CustomerViewModel(customer, Data);
            Customers.Add(defualtCustomer);
        }

        public void CustomerRemove(CustomerViewModel customer)
        {
            Customers.Remove(customer);
        }

        public ObservableCollection<CustomerViewModel> CustomerList()
        {
            return Customers;
        }

        public string FilterValue = "";
        public async void Filter()
        {
            foreach (var customer in Customers)
            {
                customer.SaveCustomer();
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

        internal void Load()
        {
            throw new NotImplementedException();
        }

        private List<CustomerViewModel> filteredList = new();



        private void FilterByName(string FilterText, CustomerValueObject customer)
        {
            if (customer.FirstName.Contains(FilterText) == false && customer.LastName.Contains(FilterText) == false)
            {
                return;
            }

            var model = new CustomerViewModel(customer, Data);
            if (!filteredList.Contains(model))
            {
                filteredList.Add(model);
            }
        }

        
        private CustomerViewModel selectedCustomer;
        

    }
}
