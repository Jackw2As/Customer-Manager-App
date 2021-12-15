using CustomerManagerApp.ViewModel;
using System.Windows.Controls;

namespace CustomerManagerApp.Wpf.Controls
{
    public sealed partial class UserEditView : UserControl
    {
        public EditViewModel ViewModel { get; set; }
        public UserEditView()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
    }
}
