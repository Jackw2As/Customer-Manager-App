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

            ViewModel.RefreshList();
        }

        private void Refresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.RefreshList();
        }

        private void new_customer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.AddNewCustomerToList();
        }

        private void filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var test = ViewModel.FilterValue;
            ViewModel.Filter();
        }
    }
}
