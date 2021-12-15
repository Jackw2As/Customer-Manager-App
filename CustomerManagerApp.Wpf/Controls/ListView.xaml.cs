using CustomerManagerApp.ViewModel;
using System.Windows.Controls;

namespace CustomerManagerApp.Wpf.Controls
{
    public sealed partial class ListView : UserControl
    {
        public ListViewModel ViewModel { get; set; }
        public ListView()
        {
            this.DataContext = ViewModel;
            this.InitializeComponent();
        }
    }
}
