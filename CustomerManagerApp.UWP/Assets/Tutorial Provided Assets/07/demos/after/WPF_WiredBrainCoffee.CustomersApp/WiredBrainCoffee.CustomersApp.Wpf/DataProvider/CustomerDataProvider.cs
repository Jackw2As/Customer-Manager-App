using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using WiredBrainCoffee.CustomersApp.Wpf.Model;
using Newtonsoft.Json;

namespace WiredBrainCoffee.CustomersApp.Wpf.DataProvider
{
  public class CustomerDataProvider
  {
    private static readonly string _customersFileName = "customers.json";

    public Task<IEnumerable<Customer>> LoadCustomersAsync()
    {
      IEnumerable<Customer> customerList;
      if (!File.Exists(_customersFileName))
      {
        customerList = new List<Customer>
        {
          new Customer{FirstName="Thomas",LastName="Huber",IsDeveloper=true},
          new Customer{FirstName="Anna",LastName="Rockstar",IsDeveloper=true},
          new Customer{FirstName="Julia",LastName="Master"},
          new Customer{FirstName="Urs",LastName="Meier",IsDeveloper=true},
          new Customer{FirstName="Sara",LastName="Ramone"},
          new Customer{FirstName="Elsa",LastName="Queen"},
          new Customer{FirstName="Alex",LastName="Baier", IsDeveloper=true},
        };
      }
      else
      {
        var json = File.ReadAllText(_customersFileName);
        customerList = JsonConvert.DeserializeObject<List<Customer>>(json);
      }

      return Task.FromResult(customerList);
    }

    public Task SaveCustomersAsync(IEnumerable<Customer> customers)
    {
      var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
      File.WriteAllText(_customersFileName, json);
      return Task.CompletedTask;
    }
  }
}