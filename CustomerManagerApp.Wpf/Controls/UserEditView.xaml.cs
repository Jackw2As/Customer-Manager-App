using CustomerManagerApp.ViewModel;
using System.Windows.Controls;

namespace CustomerManagerApp.Wpf.Controls
{
    public sealed partial class UserEditView : UserControl
    {
        public MainWindowViewModel ViewModel { get; set; }
        public UserEditView()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
    }
}
