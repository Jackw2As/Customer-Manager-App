﻿using CustomerManagerApp.Backend.Service;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CustomerManagerApp.WinUI.Wrapper;

namespace CustomerManagerApp.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		private readonly DataService dataService;

		public MainWindowViewModel(DataService DataService)
		{
			this.dataService = DataService;

			ListViewModel = new(DataService);
			EditViewModel = new(DataService);

            EditViewModel.RemoveSelectedCustomerEvent += EditViewModel_RemoveSelectedCustomerEvent;
            EditViewModel.SaveSelectedCustomerEvent += EditViewModel_SaveSelectedCustomerEvent;

            ListViewModel.SelectedCustomerRaisedEvent += ListViewModel_SelectedCustomerRaisedEvent;
            ListViewModel.OnRefreshRaised += ListViewModel_OnRefreshRaised;
		}

        private void ListViewModel_OnRefreshRaised() => Load();
        private void ListViewModel_SelectedCustomerRaisedEvent(CustomerWrapper customer) => EditViewModel.SelectedCustomer = customer;
        

        private void EditViewModel_SaveSelectedCustomerEvent(CustomerWrapper customer)
        {
            //Save
			//Do I want to reload data?
        }

        private void EditViewModel_RemoveSelectedCustomerEvent(CustomerWrapper customer)
        {
			ListViewModel.SelectedCustomer = null;
        }

        //Child View Models
        public ListViewModel ListViewModel { get; set; }
		public EditViewModel EditViewModel { get; set; }



		public bool IsLoading { get; private set; }  = false;
		public void Load()
		{
			//stops multiple refreshes firing at once.
			if (IsLoading) return;
			IsLoading = true;

            try
            {
				ListViewModel.Load();
				EditViewModel.Load();
			}
			catch(NotImplementedException ex)
            {
				Debug.Print($"{ex.GetType} exception recorded: On {ex.Source}, {ex.TargetSite} isn't supported yet.");
            }

			IsLoading = false;
		}
	}
}