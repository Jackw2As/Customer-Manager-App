using CustomerManagerApp.Backend.Entity;
using CustomerManagerApp.Backend.Services.CustomerDataLoader;
using CustomerManagerApp.Backend.Services.DrinkRoleLoader;
using CustomerManagerApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            var customerData = new CustomerDataContainer(new CustomerDataJsonLoader());
            viewModel = new(customerData, new MockDrinkTypesLoader());
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
