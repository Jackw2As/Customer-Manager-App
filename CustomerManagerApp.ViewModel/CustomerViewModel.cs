using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CustomerManagerApp.Backend.Entity;
using CustomerManagerApp.Backend.Model;

namespace CustomerManagerApp.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        private Customer customer { get; init; }
        private CustomerDataContainer CustomerLoader { get; }

        public CustomerViewModel(   Customer Customer,  
                                    CustomerDataContainer customerLoader)
        {
            customer = Customer;
            CustomerLoader = customerLoader;
        }

        public string FirstName 
        { 
            get => customer.FirstName; 
            set
            {
                if(value != null && value != customer.FirstName)
                {
                    customer.FirstName = value;
                    PropertyHasChanged();
                    PropertyHasChanged(nameof(DisplayName));
                }
            } 
        }

        public string LastName
        {
            get => customer.LastName;
            set
            {
                if (value != null && value != customer.LastName)
                {
                    customer.LastName = value;
                    PropertyHasChanged();
                    PropertyHasChanged(nameof(DisplayName));
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
                    PropertyHasChanged();
                    PropertyHasChanged(nameof(CanSave));
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
                    PropertyHasChanged();
                    PropertyHasChanged(nameof(CanSave));
                }
            }
        }

        public string DrinkId
        {
            get => customer.DrinkID;
            set
            {
                if (value != null && value != DrinkId)
                {
                    customer.DrinkID = value;
                }
            }
        }

        public string DisplayName { get => FirstName + " " + LastName; }

        public bool CanSave { 
            get 
            {
                if (IsSaving == false)
                {
                    //Model Validation Logic
                    if (!string.IsNullOrWhiteSpace(FirstName))
                    {
                        if (!string.IsNullOrWhiteSpace(LastName))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool IsSaving { get; private set; } = false;

        internal void Remove()
        {
            CustomerLoader.Remove(customer);
        }

        public void SaveCustomer()
        {
            IsSaving = true;

            CustomerLoader.Save(customer);

            IsSaving = false;
        }
    }
}
