using System.Windows.Controls;

namespace CustomerManagerApp.Wpf.CustomerList
{
    public sealed partial class CustomerListView : UserControl
    {
        public CustomerListView()
        {
            DataContext = new CustomerListViewModel();
            InitializeComponent();
        }
    }
}
