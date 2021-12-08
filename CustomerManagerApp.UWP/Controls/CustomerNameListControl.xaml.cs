using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CustomerManagerApp.UWP.Controls
{
    public delegate void MoveLocationHandler(object sender, RoutedEventArgs e, SymbolIcon symbolIcon);
    public delegate void ChangeCustomerDetailsViewedHandler(Customer customerDetails);

    public sealed partial class CustomerNameListControl : UserControl
    {
        public event ChangeCustomerDetailsViewedHandler CustomerDetailsChanged;
        public event MoveLocationHandler MoveLocation;
        public CustomerNameListControl()
        {
            this.InitializeComponent();
        }

        private void customerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var customer = customerList.SelectedItem as Customer;
            CustomerDetailsChanged.Invoke(customer);
            
        }

        private void buttonAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer("", "", false);
            customerList.Items.Add(customer);
            customerList.SelectedItem = customer;
        }

        private async void buttonRemoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (customerList.SelectedItem as Customer == null)
            {
                var messageDialog = new MessageDialog("No Customer Selected!");
                await messageDialog.ShowAsync();
                return;
            }
            customerList.Items.Remove(customerList.SelectedItem);
        }

        private void buttonMove_Click(object sender, RoutedEventArgs e) {
            MoveLocation?.Invoke(sender, e, moveSymbolIcon);
        }

        public void SetDisplayedCustomerData(IEnumerable<Customer> CustomerList)
        {
            customerList.Items.Clear();

            foreach (var customer in CustomerList as List<Customer>)
            {
                customerList.Items.Add(customer);
            }
        }

        public IEnumerable<Customer> GetDisplayedCustomerData()
        {
            return customerList.Items.OfType<Customer>();
        }

    }
}
