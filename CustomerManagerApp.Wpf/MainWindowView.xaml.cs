using CustomerManagerApp.Backend.Service;
using System.Windows;

namespace CustomerManagerApp.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowViewModel ViewModel { get; set; }
        public MainWindowView()
        {
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
            this.Loaded += MainWindowView_Loaded;
            InitializeComponent();
        }

        private void MainWindowView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Load();
        }
    }
}
