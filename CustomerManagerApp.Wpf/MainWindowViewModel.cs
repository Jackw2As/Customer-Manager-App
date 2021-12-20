using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.Backend.ValueObjects;
using CustomerManagerApp.Wpf.CustomerEdit;
using CustomerManagerApp.Wpf.CustomerList;
using CustomerManagerApp.Wpf.Wrapper;
using System.Collections.ObjectModel;

namespace CustomerManagerApp.Wpf
{
    public class MainWindowViewModel : ViewModelBase
    {
        //This Class should handle the creations ViewModels under the Main Window
        //This Class should pass data between the viewModels.
        //This Class should handle taking to any external classess.



        //Construct Child View Models
        internal CustomerEditViewModel EditViewModel;
        internal CustomerListViewModel ListViewModel;


        public MainWindowViewModel()
        {
            DataService DataService = new();
            EditViewModel = new(ref DataService);

            ListViewModel.SelectedCustomerChanged += SelectedCustomerChangedEvent;
            ListViewModel.OnRefresh += ListViewModel_OnRefreshRaised;

            EditViewModel.RemoveCustomerSelected += EditViewModel_RemoveSelectedCustomerEvent;
        }

        //Handle View Model Commands
        private void EditViewModel_RemoveSelectedCustomerEvent(CustomerWrapper customer) => Load();
        private void ListViewModel_OnRefreshRaised() => Load();


        private void SelectedCustomerChangedEvent(CustomerWrapper? customer) => EditViewModel.SelectedCustomer = customer;

        public bool IsLoading { get; private set; } = false;
        public void Load()
        {
            //stops multiple refreshes firing at once.
            if (IsLoading) return;
            IsLoading = true;

            ListViewModel.Load();
            EditViewModel.Load();

            IsLoading = false;
        }
    }
}
