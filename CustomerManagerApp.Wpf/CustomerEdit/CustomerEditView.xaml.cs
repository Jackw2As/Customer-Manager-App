using System.Windows.Controls;
using System.Windows;

namespace CustomerManagerApp.Wpf.CustomerEdit
{
    public sealed partial class CustomerEditView : UserControl
    {
        public CustomerEditViewModel ViewModel { get; set; }
        public CustomerEditView(CustomerEditViewModel viewModel)
        {
            DataContext = viewModel;
            ViewModel = viewModel;

            InitializeComponent();

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ViewModel == null) return;
            if (e.PropertyName == nameof(CustomerEditViewModel.IsCustomerSelected))
            {
                if(ViewModel.IsCustomerSelected)
                {
                    EditView_Panel.Visibility = Visibility.Visible;
                }
                else
                {
                    EditView_Panel.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
