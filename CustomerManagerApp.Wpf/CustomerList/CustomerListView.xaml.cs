using System.Windows.Controls;

namespace CustomerManagerApp.Wpf.CustomerList
{
    public sealed partial class CustomerListView : UserControl
    {
        public CustomerListViewModel ViewModel { get; set; }
        public CustomerListView(CustomerListViewModel viewModel)
        {
            DataContext = viewModel;
            ViewModel = viewModel;

            InitializeComponent();

            list.ItemsSource = viewModel.FilteredCustomerList;
            list.SelectedItem = viewModel.SelectedCustomer;
        }
    }
}
