using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WiredBrainCoffee.CustomersApp.Wpf.DataProvider;
using WiredBrainCoffee.CustomersApp.Wpf.Model;

namespace WiredBrainCoffee.CustomersApp.Wpf
{
  public partial class MainWindow : Window
  {
    private CustomerDataProvider _customerDataProvider;

    public MainWindow()
    {
      this.InitializeComponent();
      this.Loaded += MainWindow_Loaded;
      this.Closing += MainWindow_Closing;
      _customerDataProvider = new CustomerDataProvider();
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
      customerListView.Items.Clear();

      var customers = await _customerDataProvider.LoadCustomersAsync();
      foreach (var customer in customers)
      {
        customerListView.Items.Add(customer);
      }
    }

    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      _customerDataProvider.SaveCustomersAsync(
        customerListView.Items.OfType<Customer>()).Wait();
    }

    private void ButtonAddCustomer_Click(object sender, RoutedEventArgs e)
    {
      var customer = new Customer { FirstName = "New" };
      customerListView.Items.Add(customer);
      customerListView.SelectedItem = customer;
    }

    private void ButtonDeleteCustomer_Click(object sender, RoutedEventArgs e)
    {
      var customer = customerListView.SelectedItem as Customer;
      if (customer != null)
      {
        customerListView.Items.Remove(customer);
      }
    }

    private void ButtonMove_Click(object sender, RoutedEventArgs e)
    {
      int column = Grid.GetColumn(customerListGrid);

      int newColumn = column == 0 ? 2 : 0;

      Grid.SetColumn(customerListGrid, newColumn);
    }

    private void CustomerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var customer = customerListView.SelectedItem as Customer;
      customerDetailControl.Customer = customer;
    }
  }
}
