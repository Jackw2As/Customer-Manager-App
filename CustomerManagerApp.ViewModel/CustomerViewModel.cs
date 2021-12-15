using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.Backend.ValueObjects;

namespace CustomerManagerApp.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        private CustomerValueObject customer { get; init; }
        private DataService customerData { get; }

        public CustomerViewModel(   CustomerValueObject Customer,
                                    DataService DataService)
        {
            customer = Customer;
            customerData = DataService;
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

        internal async void Remove()
        {
            await customerData.RemoveCustomerAsync(customer);
        }

        public async void SaveCustomer()
        {
            IsSaving = true;

            await customerData.AddCustomerAsync(customer);

            IsSaving = false;
        }
    }
}
