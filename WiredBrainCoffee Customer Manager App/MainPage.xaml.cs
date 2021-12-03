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
        private ICustomerDataLoaderService CustomerData;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            App.Current.Suspending += App_Suspending;
            CustomerData = new CustomerDataJsonLoader();
        }

        private async void LoadCustomerData()
        {
            var customers = await CustomerData.LoadCustomersAsync() as List<Customer>;
            customerNameList.SetDisplayedCustomerData(customers);
        }

        private async void SaveCustomerData()
        {
            var customerData = customerNameList.GetDisplayedCustomerData();
            await CustomerData.SaveCustomerAsync(customerData);
        }

        //event fired when this object is loaded.
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomerData();

            customerNameList.MoveLocation += CustomerNameList_MoveLocation;
            customerNameList.CustomerDetailsChanged += CustomerNameList_CustomerDetailsChanged;
        }

        private void CustomerNameList_CustomerDetailsChanged(Customer customerDetails)
        {
            CustomerDetailControl.Customer = customerDetails;
        }

        private void CustomerNameList_MoveLocation(object sender, RoutedEventArgs e, SymbolIcon symbolIcon)
        {
            int column = Grid.GetColumn(customerNameList);
            int newColumn = column == 0 ? 2 : 0;
            symbolIcon.Symbol = newColumn == 0 ? Symbol.Forward : Symbol.Back;
            Grid.SetColumn(customerNameList, newColumn);
        }

        //Saves Customer List when App becomes suspended.
        private void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            SaveCustomerData();
            deferral.Complete();
        }
    }
}
