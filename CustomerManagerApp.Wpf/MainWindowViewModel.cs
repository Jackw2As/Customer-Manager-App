using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Wpf.CustomerEdit;
using CustomerManagerApp.Wpf.CustomerList;
using CustomerManagerApp.Wpf.Wrapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace CustomerManagerApp.Wpf
{
    public class MainWindowViewModel : ViewModelBase
    {

        //Child ViewModels
        public CustomerEditViewModel CustomerEdit { get; set; }
        public CustomerListViewModel CustomerList { get; set; }

        private DrinkWrapper defaultdrink = new();

        DataService dataService;
        public MainWindowViewModel()
        {
            CreateDataService();
            GetDefaultDrink();
            
            while(CustomerList == null && CustomerEdit == null)
            {
                Thread.Sleep(100);
            }

            CustomerList.SelectedCustomerChanged += SelectedCustomerChangedEvent;
            CustomerList.OnRefresh += ListViewModel_OnRefreshRaised;

            CustomerEdit.RemoveCustomerSelected += EditViewModel_RemoveSelectedCustomerEvent;
            

            async void CreateDataService()
            {
                dataService = await DataService.CreateDataServiceObjectAsync();

                CustomerList = new(ref dataService, defaultdrink);
                CustomerEdit = new(ref dataService);
            }
        }

        

        private async void GetDefaultDrink()
        {
            var drinks = await dataService.GetDrinksAsync();
            new DrinkWrapper(drinks.First());
        }

        //Handle View Model Commands
        private void EditViewModel_RemoveSelectedCustomerEvent(CustomerWrapper? customer) => Load();
        private void ListViewModel_OnRefreshRaised() => Load();


        private void SelectedCustomerChangedEvent(CustomerWrapper? customer) => CustomerEdit.SelectedCustomer = customer;

        public bool IsLoading { get; private set; } = false;
        public void Load()
        {
            //stops multiple refreshes firing at once.
            if (IsLoading) return;
            IsLoading = true;

            CustomerList.Load();
            CustomerEdit.Load();

            IsLoading = false;
        }
    }
}
