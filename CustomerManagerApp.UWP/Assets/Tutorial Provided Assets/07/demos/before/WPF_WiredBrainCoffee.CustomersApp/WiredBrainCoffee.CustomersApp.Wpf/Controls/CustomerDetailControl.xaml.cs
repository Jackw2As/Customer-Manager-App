using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using WiredBrainCoffee.CustomersApp.Wpf.Model;

namespace WiredBrainCoffee.CustomersApp.Wpf.Controls
{
  [ContentProperty(nameof(Customer))]
  public sealed partial class CustomerDetailControl : UserControl
  {
    private Customer _customer;
    private bool _isSettingCustomer = false;

    public CustomerDetailControl()
    {
      this.InitializeComponent();
    }

    public Customer Customer
    {
      get { return _customer; }
      set
      {
        _customer = value;
        _isSettingCustomer = true;
        txtFirstName.Text = _customer?.FirstName ?? "";
        txtLastName.Text = _customer?.LastName ?? "";
        chkIsDeveloper.IsChecked = _customer?.IsDeveloper;
        _isSettingCustomer = false;
      }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      UpdateCustomer();
    }

    private void CheckBox_IsCheckedChanged(object sender, RoutedEventArgs e)
    {
      UpdateCustomer();
    }

    private void UpdateCustomer()
    {
      if (_isSettingCustomer)
      {
        return;
      }

      if (Customer != null)
      {
        Customer.FirstName = txtFirstName.Text;
        Customer.LastName = txtLastName.Text;
        Customer.IsDeveloper = chkIsDeveloper.IsChecked.GetValueOrDefault();
      }
    }
  }
}
