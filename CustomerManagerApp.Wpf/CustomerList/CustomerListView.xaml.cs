using System.Windows.Controls;

namespace CustomerManagerApp.Wpf.CustomerList
{
    public sealed partial class CustomerListView : UserControl
    {
        public CustomerListView()
        {
            InitializeComponent();

            CustomerListViewModel? viewModel = DataContext as CustomerListViewModel;
            if (viewModel == null) throw new System.Exception("Should never be null");

            list.ItemsSource = viewModel.FilteredCustomerList;
            list.SelectedItem = viewModel.SelectedCustomer;
        }
    }
}
