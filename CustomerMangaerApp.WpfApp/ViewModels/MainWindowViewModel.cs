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
        private readonly DataService dataService;
        private CustomerListViewModel? customerList;
        private CustomerEditViewModel? customerEdit;

        //constructors
        public MainWindowViewModel()
        {
            this.dataService = new DataService();
        }
        public MainWindowViewModel(DataService dataService)
        {
            this.dataService = dataService;
        }

        //properties
        public CustomerEditViewModel? CustomerEdit
        {
            get => customerEdit; 
            set {                
                customerEdit = value;
                if(value != null)
                value.RemoveCustomerSelected += EditViewModel_RemoveSelectedCustomerEvent;
            }
        }
        public CustomerListViewModel? CustomerList
        {
            get => customerList; 
            set {

                customerList = value;
                if(value != null)
                {
                    value.SelectedCustomerChanged += SelectedCustomerChangedEvent;
                    value.OnRefresh += ListViewModel_OnRefreshRaised;
                }
            }
        }

        public bool IsLoading { get; private set; } = false;
        
        //methods
        public async Task Load()
        {
            //stops multiple refreshes firing at once.
            if (IsLoading) return;
            IsLoading = true;

            if (CustomerList != null)
            {
                await CustomerList.Load();
            }
            CustomerEdit?.Load();

            IsLoading = false;
        }

        //local methods

        private void Remove(CustomerWrapper? customer)
        {
            if (customer == null) return;
            dataService.RemoveCustomerFromList(customer.GetWrappedCustomer);
            if(customerList != null)
            customerList.RemoveCustomer(customer);
        }
        private void EditViewModel_RemoveSelectedCustomerEvent(CustomerWrapper? customer) => Remove(customer);
        private async Task ListViewModel_OnRefreshRaised() => await Load();
        private async Task SelectedCustomerChangedEvent(CustomerWrapper? customer) => await CustomerEdit!.CustomerSelected(customer);
    }
}
