using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WiredBrainCoffee_Customer_Manager_App.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WiredBrainCoffee_Customer_Manager_App.Controls
{
    public sealed partial class CustomerDetailControl : UserControl
    {
        public CustomerDetailControl()
        {
            this.InitializeComponent();
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { 
                _customer = value;
                cdFirstName.Text = _customer?.FirstName ?? "";
                cdLastName.Text = _customer?.LastName ?? "";
                cdisDeveloper.IsChecked = _customer?.IsDeveloper ?? false;
            }
        }

        private async void cd_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCustomer();
        }

        private void cd_isDeveloperChanged(object sender, RoutedEventArgs e)
        {
            UpdateCustomer();
        }
        private void UpdateCustomer()
        {
            if (Customer != null)
            {
                Customer.FirstName = cdFirstName.Text;
                Customer.LastName = cdLastName.Text;
                Customer.IsDeveloper = cdisDeveloper.IsChecked.GetValueOrDefault();
            }
        }
    }
}
