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
using WiredBrainCoffee_Customer_Manager_App.Model;
using WiredBrainCoffee_Customer_Manager_App.Services.CustomerDataLoader;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WiredBrainCoffee_Customer_Manager_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ICustomerDataLoaderService customerData;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            App.Current.Suspending += App_Suspending;
            customerData = new CustomerDataJsonLoader();
        }

        private async void LoadCustomerData()
        {
            CustomerList.Items.Clear();

            var customers = await customerData.LoadCustomersAsync() as List<Customer>;
            foreach (var customer in customers)
            {
                CustomerList.Items.Add(customer);
            }
        }

        private async void SaveCustomerData()
        {
            await customerData.SaveCustomerAsync(CustomerList.Items.OfType<Customer>());
        }

        //event fired when this object is loaded.
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomerData();
        }

        

        //Saves Customer List when App becomes suspended.
        private void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            SaveCustomerData();
            deferral.Complete();
        }

        private void ButtonAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer("", "", false);
            CustomerList.Items.Add(customer);
            CustomerList.SelectedItem = customer;
        }

        private async void ButtonRemoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            if(CustomerList.SelectedItem as Customer == null)
            {
                var messageDialog = new MessageDialog("No Customer Selected!");
                await messageDialog.ShowAsync();
                return;
            }
            CustomerList.Items.Remove(CustomerList.SelectedItem);
        }
        private void ButtonMove_Click(object sender, RoutedEventArgs e)
        {
            int column = Grid.GetColumn(customerListGrid);
            int newColumn = column == 0 ? 2 : 0;
            
            Grid.SetColumn(customerListGrid, newColumn);
            moveSymbolIcon.Symbol = newColumn == 0 ? Symbol.Forward : Symbol.Back;
        }
        
        private void CustomerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var customer = CustomerList.SelectedItem as Customer;
            CustomerDetailControl.Customer = customer;
        }
    }
}
