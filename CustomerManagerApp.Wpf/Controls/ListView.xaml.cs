using CustomerManagerApp.ViewModel;
using System.Windows.Controls;

namespace CustomerManagerApp.Wpf.Controls
{
    public sealed partial class ListView : UserControl
    {
        public MainWindowViewModel ViewModel { get; set; }
        public ListView()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
            Loaded += ListView_Loaded;
        }

        private void ListView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.Load();
        }
    }
}
