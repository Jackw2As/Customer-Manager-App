using CustomerManagerApp.WpfApp.ViewModels;
using System.Threading.Tasks;
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

            Task.Run(()=>ViewModel.RefreshList());
        }

        private async void Refresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await ViewModel.RefreshList();
        }

        private async void new_customer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await ViewModel.AddNewCustomerToList();
        }

        private void filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var test = ViewModel.FilterValue;
            ViewModel.Filter();
        }

    }
}
