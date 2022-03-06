using CustomerManagerApp.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Service.FilterService
{
    public interface IFilterService
    {
        public List<CustomerEntity> FilterCustomerList(List<CustomerEntity> listToFilter, string filterValue);
    }
}
