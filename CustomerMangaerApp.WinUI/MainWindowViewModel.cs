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

        private readonly DataService dataService;

        public ObservableCollection<DrinkValueObject> DrinkTypes { get; } = new();

        public MainWindowViewModel(DataService DataService)
        {
            this.dataService = DataService;
            this.DrinkTypes = new(DataService.GetDrinksAsync().Result);

            ListViewModel = new(DataService);
            EditViewModel = new(DrinkTypes);
        }

        public ListViewModel ListViewModel { get; set; }
        public EditViewModel EditViewModel { get; set; }
        public bool IsLoading { get; private set; }  = false;
        public async void Load()
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
