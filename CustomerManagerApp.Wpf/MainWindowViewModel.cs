﻿using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Wpf.CustomerEdit;
using CustomerManagerApp.Wpf.CustomerList;
using CustomerManagerApp.Wpf.Wrapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CustomerManagerApp.Wpf
{
    public class MainWindowViewModel : ViewModelBase
    {

        //Child ViewModels
        public CustomerEditViewModel CustomerEdit { get; set; }
        public CustomerListViewModel CustomerList { get; set; }

        public MainWindowViewModel()
        {
            DataService DataService = new();       
            DrinkWrapper defaultdrink = new DrinkWrapper(DataService.GetDrinks().First());

            CustomerList = new(ref DataService, defaultdrink);
            CustomerEdit = new(ref DataService);

            CustomerList.SelectedCustomerChanged += SelectedCustomerChangedEvent;
            CustomerList.OnRefresh += ListViewModel_OnRefreshRaised;

            CustomerEdit.RemoveCustomerSelected += EditViewModel_RemoveSelectedCustomerEvent;
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