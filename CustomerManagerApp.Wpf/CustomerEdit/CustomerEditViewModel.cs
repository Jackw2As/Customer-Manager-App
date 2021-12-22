using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.Backend.ValueObjects;
using CustomerManagerApp.Wpf.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomerManagerApp.Wpf.CustomerEdit
{
    internal delegate void RemoveSelectedCustomer(CustomerWrapper? customer);
    internal delegate void SaveSelectedCustomer(CustomerWrapper? customer);
    public class CustomerEditViewModel : ViewModelBase
    {
        public ObservableCollection<DrinkValueObject> DrinkTypes { get; init; } = new();

        private CustomerWrapper? selectedCustomer;
        private readonly DataService data;

        public CustomerWrapper? SelectedCustomer 
        { 
            get => selectedCustomer;
            set
            { 
                if (value != selectedCustomer)
                {
                    selectedCustomer = value;
                    PropertyHasChanged();
                    PropertyHasChanged(nameof(IsCustomerSelected));

                }
            }
        }

        public CustomerEditViewModel(ref DataService Data)
        {
            data = Data;
            var drinks = Data.GetDrinks();

            foreach (var drink in drinks)
            {
                DrinkTypes.Add(drink);
            }
        }

        public CustomerEditViewModel()
        {
            data = new DataService();
        }

        public bool CanSave
        {
            get
            {
                if (IsCustomerSelected)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsCustomerSelected
        {
            get
            {
                if (SelectedCustomer != null)
                {
                    return true;
                }

                return false;
            }
        }

        internal void Load()
        {
            SelectedCustomer = null;
        }

        internal event RemoveSelectedCustomer? RemoveCustomerSelected;
        
        public void RemoveSelectedCustomer()
        {
            if (SelectedCustomer != null)
            {
                RemoveCustomerSelected?.Invoke(SelectedCustomer);
                SelectedCustomer = null;
            }
        }

        internal event SaveSelectedCustomer? SaveSelectedCustomer;
        public async void SaveCustomer()
        {
            if (SelectedCustomer != null)
            {
                await data.UpdateCustomerAsync(SelectedCustomer.GetWrappedCustomer);
                SelectedCustomer = null;
                SaveSelectedCustomer?.Invoke(SelectedCustomer);
                
            }
        }
    }
}
