using CustomerManagerApp.Backend.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.ViewModel
{
    public delegate void RemoveSelectedCustomer(CustomerViewModel customer);
    public class EditViewModel : ViewModelBase
    {
        public ObservableCollection<DrinkValueObject> DrinkTypes { get; init; } = new();

        public EditViewModel(ObservableCollection<DrinkValueObject> drinkTypes)
        {
            if (drinkTypes == null)
            {
                DrinkTypes = drinkTypes;
            }
        }

        private CustomerViewModel selectedCustomer;
        public CustomerViewModel SelectedCustomer
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
                }
                else
                {
                    if (selectedCustomer != null)
                    {
                        PropertyHasChanged();
                        PropertyHasChanged(nameof(IsCustomerSelected));
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

        public event RemoveSelectedCustomer RemoveSelectedCustomerEvent;
        public void RemoveSelectedCustomer()
        {
            if(SelectedCustomer != null)
            {
                SelectedCustomer.Remove();
                RemoveSelectedCustomerEvent?.Invoke(SelectedCustomer);
            }
        }

        internal void Load()
        {
            throw new NotImplementedException();
        }
    }
}
