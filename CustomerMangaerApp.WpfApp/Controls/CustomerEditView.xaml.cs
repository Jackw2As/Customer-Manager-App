using System.Windows.Controls;
using System.Windows;
using CustomerManagerApp.WpfApp.ViewModels;

namespace CustomerManagerApp.WpfApp.Controls
{
    public sealed partial class CustomerEditView : UserControl
    {
        public CustomerEditViewModel ViewModel { get; set; }
        public CustomerEditView()
        {
            ViewModel = new();
            DataContext = ViewModel;

            InitializeComponent();

            EditView_Panel.Visibility = Visibility.Hidden;

            if (ViewModel != null)
            {
                ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ViewModel == null) return;
            if (e.PropertyName == nameof(CustomerEditViewModel.IsCustomerSelected))
            {
                if (ViewModel.IsCustomerSelected)
                {
                    EditView_Panel.Visibility = Visibility.Visible;
                }
                else
                {
                    EditView_Panel.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}