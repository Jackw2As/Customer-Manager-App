using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

namespace WiredBrainCoffee.CustomersApp
{
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();
    }

    private async void ButtonAddCustomer_Click(object sender, RoutedEventArgs e)
    {
      var messageDialog = new MessageDialog("Customer added!");
      await messageDialog.ShowAsync();
    }
  }
}
