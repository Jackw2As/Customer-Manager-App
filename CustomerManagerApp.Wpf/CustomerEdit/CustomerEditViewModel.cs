using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Backend.Service;
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
        public ObservableCollection<DrinkEntity> DrinkTypes { get; init; } = new();

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
            GetDrinks();
        }

        private async void GetDrinks()
        {
            var drinkList = await data.GetDrinksAsync();
            DrinkTypes.Clear();
            foreach (var drink in drinkList)
            {
                DrinkTypes.Add(drink);
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
        public void SaveCustomer()
        {
            if (SelectedCustomer != null)
            {
                data.AddCustomerToList(SelectedCustomer.GetWrappedCustomer);
                SelectedCustomer = null;
                SaveSelectedCustomer?.Invoke(SelectedCustomer);
                
            }
        }
    }
}
