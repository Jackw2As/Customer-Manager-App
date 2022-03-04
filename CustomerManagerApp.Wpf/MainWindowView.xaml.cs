using CustomerManagerApp.Backend.Service;
using System.Threading.Tasks;
using System.Windows;

namespace CustomerManagerApp.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowViewModel? ViewModel { get; set; }
        public MainWindowView()
        {
            Task.Factory.StartNew(()=>Load());

        }

        private async Task Load()
        {
            var dataContainer = await DataService.CreateDataServiceObjectAsync();
            ViewModel = new MainWindowViewModel(dataContainer);
            ViewModel.Load();
            DataContext = ViewModel;
            this.InitializeComponent();
        }
    }
}
