﻿using CustomerManagerApp.Backend.Service;
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
        //In the Constructor what I want to happen.
        //Create a CustomerEdit and CustomerList Event.
        //Create two seperate windows from this.


        //Child ViewModels
        public CustomerEditViewModel CustomerEdit { get; set; }
        public CustomerListViewModel CustomerList { get; set; }

        private DrinkWrapper defaultdrink = new();

        private DataService dataService;
        public MainWindowViewModel(DataService dataService)
        {
            this.dataService = dataService;
            GetDefaultDrink(dataService);

            CustomerList = new(ref dataService);
            CustomerEdit = new(ref dataService);

            CustomerList.SelectedCustomerChanged += SelectedCustomerChangedEvent;
            CustomerList.OnRefresh += ListViewModel_OnRefreshRaised;

            CustomerEdit.RemoveCustomerSelected += EditViewModel_RemoveSelectedCustomerEvent;
        }

        

        private async void GetDefaultDrink(DataService service)
        {
            var drinks = await service.GetDrinksAsync();
            new DrinkWrapper(drinks.First());
        }

        //Event Handling
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
