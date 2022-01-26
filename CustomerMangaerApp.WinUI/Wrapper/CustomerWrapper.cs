using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CustomerManagerApp.Backend.Entities;
using CustomerMangaerApp.WinUI.Wrapper;

namespace CustomerManagerApp.WinUI.Wrapper
{
    public class CustomerWrapper : BaseWrapper
    {
        private readonly CustomerEntity customer;
        

        public CustomerWrapper(DrinkWrapper drinkOfChoice)
        {
            customer = new("", "", drinkOfChoice.Id.ToString());
        }

        public CustomerWrapper(CustomerEntity customer)
        {
            this.customer = customer;
        }
        public CustomerEntity GetWrappedCustomer => customer;
        public Guid Id
        {
            get => new(customer.Id);
        }

        public string FirstName
        {
            get => customer.FirstName;
            set
            {
                if (value != customer.FirstName)
                {
                    customer.FirstName = value;
                    propertyChanged();
                }
            }
        }

        public string LastName
        {
            get => customer.LastName;
            set
            {
                if (value != customer.LastName)
                {
                    customer.LastName = value;
                    propertyChanged();
                }
            }
        }

        public string DisplayName => FirstName + " " + LastName;

        public string DrinkId
        {
            get => new(customer.DrinkID);
            set
            {
                if (value != null)
                {
                    if (value.ToString() != customer.DrinkID)
                    {
                        customer.DrinkID = value.ToString();
                        propertyChanged();
                    }
                }
            }
        }

        public bool IsDeveloper
        {
            get => customer.IsDeveloper;
            set
            {
                if (value != customer.IsDeveloper)
                {
                    customer.IsDeveloper = value;
                    propertyChanged();
                }
            }
        }

        public DateTimeOffset FirstTime
        {
            get => customer.FirstTime;
            set
            {
                if (value != customer.FirstTime)
                {
                    customer.FirstTime = value;
                    propertyChanged();
                }
            }
        }
    }
}
