using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.ViewModel;
using System.Windows;

namespace CustomerManagerApp.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        MainWindowViewModel viewModel;
        public MainWindowView()
        {
            var dataService = new DataService();
            viewModel = new(dataService);
            DataContext = viewModel;
            this.Loaded += MainWindowView_Loaded;
            InitializeComponent();
        }

        private void MainWindowView_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.Load();
        }
    }
}
