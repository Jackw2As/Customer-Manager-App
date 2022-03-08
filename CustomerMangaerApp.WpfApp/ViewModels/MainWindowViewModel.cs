using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.Backend.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using CustomerManagerApp.WpfApp.Wrapper;
using System;
using System.Threading.Tasks;

namespace CustomerManagerApp.WpfApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //fields
        private DrinkWrapper defaultdrink = new();
        private DataService dataService;
        private CustomerListViewModel? customerList;
        private CustomerEditViewModel? customerEdit;

        //constructors
        public MainWindowViewModel()
        {
            Task.Run(()=>SetupModel());
        }

        //properties
        public CustomerEditViewModel? CustomerEdit
        {
            get => customerEdit; 
            set {
                if (value == null) throw new ArgumentNullException(nameof(value)); 
                
                customerEdit = value;
                value.RemoveCustomerSelected += EditViewModel_RemoveSelectedCustomerEvent;
            }
        }
        public CustomerListViewModel? CustomerList
        {
            get => customerList; 
            set {
                if (value == null) throw new ArgumentNullException(nameof(value));

                customerList = value;
                value.SelectedCustomerChanged += SelectedCustomerChangedEvent;
                value.OnRefresh += ListViewModel_OnRefreshRaised;
            }
        }

        public bool IsLoading { get; private set; } = false;
        
        //methods
        public void Load()
        {
            //stops multiple refreshes firing at once.
            if (IsLoading) return;
            IsLoading = true;

            CustomerList?.Load();
            CustomerEdit?.Load();

            IsLoading = false;
        }

        //local methods
        private async Task SetupModel()
        {
            dataService = await DataService.CreateDataServiceObjectAsync();
            var drinks = await dataService.GetDrinksAsync();
            defaultdrink = new DrinkWrapper(drinks.First());
        }

        private void Remove(CustomerWrapper? customer)
        {
            if (customer == null) return;
            dataService.RemoveCustomerFromList(customer.GetWrappedCustomer);
            if(customerList != null)
            customerList.RemoveCustomer(customer);
        }
        private void EditViewModel_RemoveSelectedCustomerEvent(CustomerWrapper? customer) => Remove(customer);
        private void ListViewModel_OnRefreshRaised() => Load();
        private void SelectedCustomerChangedEvent(CustomerWrapper? customer) => CustomerEdit!.SelectedCustomer = customer;
    }
}
