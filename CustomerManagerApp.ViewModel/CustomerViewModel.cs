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
                if(value != null)
                {
                    customer.FirstName = value;
                    PropertyHasChanged();
                }
            } 
        }

        public string LastName
        {
            get => customer.LastName;
            set
            {
                if (value != null)
                {
                    customer.LastName = value;
                    PropertyHasChanged();
                }
            }
        }

        public bool IsDeveloper
        {
            get => customer.IsDeveloper;
            set
            {
                customer.IsDeveloper = value;
                PropertyHasChanged();
            }
        }

        public DateTimeOffset FirstTime
        {
            get => customer.FirstTime;
            set
            {
                customer.FirstTime = value;
                PropertyHasChanged();
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
                    //PropertyHasChanged();
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
                            PropertyHasChanged();
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool IsSaving { get; private set; } = false;

        public void SaveCustomer()
        {
            IsSaving = true;

            CustomerLoader.Save(customer);

            IsSaving = false;
        }
    }
}
