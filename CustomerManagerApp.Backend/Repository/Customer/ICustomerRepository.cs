using CustomerManagerApp.Backend.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository.Customer
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerValueObject>> LoadCustomersAsync();

        Task SaveCustomerAsync(IEnumerable<CustomerValueObject> customers);

        void DeleteStorageFile();
    }
}
