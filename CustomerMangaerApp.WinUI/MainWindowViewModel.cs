using CustomerManagerApp.Backend.Service;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CustomerManagerApp.WinUI.Wrapper;
using CustomerManagerApp.Backend.Repository.Customer;
using System.Threading.Tasks;

namespace CustomerManagerApp.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		private readonly DataService dataService;

		public MainWindowViewModel() : this(new DataService())
        {

        }

		public MainWindowViewModel(DataService DataService)
		{
			this.dataService = DataService;

			ListViewModel = new(ref DataService);
			EditViewModel = new(ref DataService);

            EditViewModel.RemoveSelectedCustomerEvent += EditViewModel_RemoveSelectedCustomerEvent;
            EditViewModel.SaveSelectedCustomerEvent += EditViewModel_SaveSelectedCustomerEvent;

            ListViewModel.SelectedCustomerRaisedEvent += ListViewModel_SelectedCustomerRaisedEvent;
            ListViewModel.OnRefreshRaised += ListViewModel_OnRefreshRaised;
		}

        private async void ListViewModel_OnRefreshRaised() => await Load();
        private void ListViewModel_SelectedCustomerRaisedEvent(CustomerWrapper customer) => EditViewModel.SelectedCustomer = customer;
        

        private void EditViewModel_SaveSelectedCustomerEvent(CustomerWrapper customer)
        {       
		
		}

        private void EditViewModel_RemoveSelectedCustomerEvent(CustomerWrapper customer)
        {
			ListViewModel.CustomerRemove(customer);
			ListViewModel.SelectedCustomer = null;
        }

        //Child View Models
        public ListViewModel ListViewModel { get; set; }
		public EditViewModel EditViewModel { get; set; }



		public bool IsLoading { get; private set; }  = false;
		public async Task Load()
		{
			//stops multiple refreshes firing at once.
			if (IsLoading) return;
			IsLoading = true;

            try
            {
				await ListViewModel.Load();
				await EditViewModel.Load();
			}
			catch(NotImplementedException ex)
            {
				Debug.Print($"{ex.GetType} exception recorded: On {ex.Source}, {ex.TargetSite} isn't supported yet.");
            }

			IsLoading = false;
		}
	}
}
