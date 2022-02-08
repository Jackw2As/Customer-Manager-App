using CustomerManagerApp.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository.Customer
{
    public interface ICustomerRepository : IRepository<CustomerEntity, string>
    {

    }
}
