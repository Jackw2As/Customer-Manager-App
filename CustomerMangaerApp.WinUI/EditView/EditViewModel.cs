using CustomerManagerApp.Backend.Entities;
using CustomerManagerApp.Backend.Repository.Customer;
using CustomerManagerApp.Backend.Service;
using CustomerManagerApp.WinUI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.ViewModel
{
    public delegate void RemoveSelectedCustomer(CustomerWrapper customer);
    public delegate void SaveSelectedCustomer(CustomerWrapper customer);
    public class EditViewModel : ViewModelBase
    {
        public ObservableCollection<DrinkEntity> DrinkTypes { get; init; } 

        private DataService<JsonCustomerRepository> dataService;

        public EditViewModel(ref DataService<JsonCustomerRepository> DataService)
        {
            dataService = DataService;
            DrinkTypes = new(DataService.GetDrinks());
        }

        private CustomerWrapper selectedCustomer;
        public CustomerWrapper SelectedCustomer
        {
            get => selectedCustomer; 
            
            set {
                if (value != null && value != selectedCustomer)
                {
                    selectedCustomer = value;

                    selectedCustomer.FirstTime = value.FirstTime;
                    selectedCustomer.DrinkId = value.DrinkId;
                    selectedCustomer.FirstName = value.FirstName;
                    selectedCustomer.LastName = value.LastName;
                    selectedCustomer.IsDeveloper = value.IsDeveloper;

                    PropertyHasChanged();
                    PropertyHasChanged(nameof(IsCustomerSelected));
                    PropertyHasChanged(nameof(CanSave));
                }
                else
                {
                    if (selectedCustomer != null)
                    {
                        PropertyHasChanged();
                        PropertyHasChanged(nameof(IsCustomerSelected));
                        PropertyHasChanged(nameof(CanSave));
                    }

                    selectedCustomer = null;
                }
            }
        }

        public bool IsCustomerSelected
        {
            get
            {
                if(SelectedCustomer != null)
                {
                    return true;
                }
                
                return false;
            }
        }

        public bool CanSave { 
            get
            {
                if(IsCustomerSelected)
                {
                    return true;
                }
                return false;
            }
        }

        public event RemoveSelectedCustomer RemoveSelectedCustomerEvent;
        public async void RemoveSelectedCustomer()
        {
            if (SelectedCustomer == null) return;
            try
            {
                await dataService.RemoveCustomerAsync(SelectedCustomer.GetWrappedCustomer);
            }
            catch (ArgumentException ex) { }
            finally
            {
                RemoveSelectedCustomerEvent?.Invoke(SelectedCustomer);
                SelectedCustomer = null;
            }
        }

        public event SaveSelectedCustomer SaveSelectedCustomerEvent;
        public void Save()
        {
            CustomerEntity customerEntity = new(SelectedCustomer.GetWrappedCustomer);

            dataService.UpdateCustomerAsync(customerEntity);
            SaveSelectedCustomerEvent?.Invoke(SelectedCustomer);
        }

        internal void Load()
        {
            DrinkTypes.Clear();

            foreach (var drink in dataService.GetDrinks())
            {
                DrinkTypes.Add(drink);
            }

            SelectedCustomer = null;
        }
    }
}
