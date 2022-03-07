using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CustomerManagerApp.Backend.Entities;

namespace CustomerManagerApp.WpfApp.Wrapper
{
    public class CustomerWrapper : BaseWrapper
    {
        private readonly CustomerEntity customer;


        public CustomerWrapper(DrinkWrapper drinkOfChoice)
        {
            customer = new(Guid.NewGuid().ToString(), "new", "customer", drinkOfChoice.Id.ToString(), new DateTime());
        }

        public CustomerWrapper(CustomerEntity customer)
        {
            this.customer = customer;
        }
        public CustomerEntity GetWrappedCustomer => customer;
        public Guid Id
        {
            get => new(customer.ID);
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
            get => customer.DrinkID;
            set
            {
                if (value.ToString() != customer.DrinkID)
                {
                    customer.DrinkID = value.ToString();
                    propertyChanged();
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

        public DateTime FirstTime
        {
            get => customer.FirstTime.DateTime;
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
