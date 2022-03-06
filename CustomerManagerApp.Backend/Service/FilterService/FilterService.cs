using CustomerManagerApp.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Service.FilterService
{
    public class FilterService : IFilterService
    {
        private List<CustomerEntity> filteredList = new();
        public List<CustomerEntity> FilterCustomerList(List<CustomerEntity> listToFilter, string filterValue)
        {
            filteredList.Clear();

            //Nothing to Filter
            if (filterValue == string.Empty)
            {
                return listToFilter;
            }

            Parallel.ForEach(listToFilter, customer => FilterByName(filterValue.ToLower(), customer));
            return filteredList;
        }
        private void FilterByName(string FilterText, CustomerEntity customer)
        {
            if (customer.FirstName.ToLower().Contains(FilterText) == false
                && customer.LastName.ToLower().Contains(FilterText) == false)
            {
                return;
            }
            filteredList.Add(customer);
        }
    }
}
