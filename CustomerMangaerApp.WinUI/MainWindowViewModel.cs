using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.Backend.ValueObjects;
using System.Collections.ObjectModel;

namespace CustomerManagerApp.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        //This Class should handle the creations ViewModels under the Main Window
        //This Class should pass data between the viewModels.
        //This Class should handle taking to any external classess.


        private CustomerViewModel selectedCustomer;

        private readonly DataService dataService;

        public ObservableCollection<DrinkValueObject> DrinkTypes { get; } = new();

        public MainWindowViewModel(DataService DataService)
        {
            this.dataService = DataService;
            this.DrinkTypes = new(DataService.GetDrinksAsync().Result);

            ListViewModel = new(DataService);
            ListViewModel.SelectedCustomerRaisedEvent += SelectedCustomerChangedEvent;
            ListViewModel.OnRefreshRaised += ListViewModel_OnRefreshRaised;

            EditViewModel = new(DrinkTypes);
            EditViewModel.RemoveSelectedCustomerEvent += EditViewModel_RemoveSelectedCustomerEvent;


        }

        private void EditViewModel_RemoveSelectedCustomerEvent(CustomerViewModel customer)
        {
            Load();
        }

        private void ListViewModel_OnRefreshRaised()
        {
            Load();
        }

        private void SelectedCustomerChangedEvent(CustomerViewModel customer)
        {
            SelectedCustomer = customer;
        }

        public CustomerViewModel SelectedCustomer
        {
            get { return selectedCustomer; }
            set {
                if (selectedCustomer != value)
                {
                    selectedCustomer = value;
                    EditViewModel.SelectedCustomer = value;
                    PropertyHasChanged();
                    PropertyHasChanged(nameof(IsCustomerSelected));
                }
            }
        }

        public ListViewModel ListViewModel { get; set; }

        public EditViewModel EditViewModel { get; set; }
        public bool IsCustomerSelected => SelectedCustomer != null;

        public bool IsLoading { get; private set; }  = false;
        public async void Load()
        {
            //stops multiple refreshes firing at once.
            if (IsLoading) return;
            IsLoading = true;

            var customers =     await dataService.GetCustomersAsync();
            var drinkTypes =    await dataService.GetDrinksAsync();

            this.EditViewModel.DrinkTypes.Clear();
            this.ListViewModel.DrinkTypes.Clear();
            DrinkTypes?.Clear();

            this.ListViewModel.Customers.Clear();
            this.ListViewModel.FilterValue = string.Empty;

            this.EditViewModel.SelectedCustomer = null;
            this.ListViewModel.SelectedCustomer = null;

            foreach (var customer in customers)
            {
                var VM = new CustomerViewModel(customer, dataService);
                ListViewModel.Customers.Add(VM);
            }

            foreach (var drink in drinkTypes)
            {
                DrinkTypes.Add(drink);
                EditViewModel.DrinkTypes.Add(drink);
                ListViewModel.DrinkTypes.Add(drink);
            }
            


            IsLoading = false;
        }
    }
}
