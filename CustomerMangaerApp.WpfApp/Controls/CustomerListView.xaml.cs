using CustomerManagerApp.WpfApp.ViewModels;
using System.Windows.Controls;

namespace CustomerManagerApp.WpfApp.Controls
{
    public sealed partial class CustomerListView : UserControl
    {
        public CustomerListViewModel ViewModel { get; set; }
        public CustomerListView()
        {
            ViewModel = new();
            DataContext = ViewModel;

            InitializeComponent();
            
            list.ItemsSource = ViewModel.FilteredCustomerList;
            list.SelectedItem = ViewModel.SelectedCustomer;            
        }
    }
}
