using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.WpfApp.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomerManagerApp.WpfApp.ViewModels
{
    internal delegate void RemoveSelectedCustomer(CustomerWrapper? customer);
    internal delegate void SaveSelectedCustomer(CustomerWrapper? customer);
    public class CustomerEditViewModel : ViewModelBase
    {
        //fields
        private DataService data;
        private CustomerWrapper? selectedCustomer;

        //constructors
        public CustomerEditViewModel()
        {
            data = new DataService();
        }

        //properties
        public ObservableCollection<DrinkWrapper> DrinkTypes { get; init; } = new();
        public CustomerWrapper? SelectedCustomer
        {
            get => selectedCustomer;
            private set
            {
                if (value != selectedCustomer)
                {
                    selectedCustomer = value;
                    PropertyHasChanged();
                    PropertyHasChanged(nameof(IsCustomerSelected));
                }
            }
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

        //events
        internal event RemoveSelectedCustomer? RemoveCustomerSelected;
        internal event SaveSelectedCustomer? SaveSelectedCustomer;

        //methods
        internal void Load()
        {
            SelectedCustomer = null;
        }
        public void RemoveSelectedCustomer()
        {
            if (SelectedCustomer != null)
            {
                RemoveCustomerSelected?.Invoke(SelectedCustomer);
                SelectedCustomer = null;
            }
        }

        public void SaveCustomer()
        {
            if (SelectedCustomer != null && data != null)
            {
                data.AddCustomerToList(SelectedCustomer.GetWrappedCustomer);
                SaveSelectedCustomer?.Invoke(SelectedCustomer);
                SelectedCustomer = null;
            }
        }

        public async Task CustomerSelected(CustomerWrapper? selectedCustomer)
        {
            if(DrinkTypes.Count > 1)
            {
                var drinkList = await data.GetDrinksAsync();
                foreach (var drink in drinkList)
                {
                    DrinkTypes.Add(new(drink));
                }

                DrinkTypes.Clear();
            }
            SelectedCustomer = selectedCustomer;
        }
    }
}
