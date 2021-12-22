using System.Windows.Controls;
using System.Windows;

namespace CustomerManagerApp.Wpf.CustomerEdit
{
    public sealed partial class CustomerEditView : UserControl
    {
        CustomerEditViewModel? viewModel;
        public CustomerEditView()
        {
            InitializeComponent();
            viewModel = DataContext as CustomerEditViewModel;
            if (viewModel == null) throw new System.Exception("ViewModle should not be null");

            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (viewModel == null) return;
            if (e.PropertyName == nameof(CustomerEditViewModel.IsCustomerSelected))
            {
                if(viewModel.IsCustomerSelected)
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
