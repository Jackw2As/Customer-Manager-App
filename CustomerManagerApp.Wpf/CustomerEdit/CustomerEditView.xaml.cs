using System.Windows.Controls;

namespace CustomerManagerApp.Wpf.CustomerEdit
{
    public sealed partial class CustomerEditView : UserControl
    {
        public CustomerEditView()
        {
            this.DataContext = new CustomerEditViewModel();
            InitializeComponent();
        }
    }
}
