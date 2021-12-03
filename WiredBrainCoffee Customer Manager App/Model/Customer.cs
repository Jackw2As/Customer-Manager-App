using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WiredBrainCoffee_Customer_Manager_App.Model
{
    public class Customer : INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;
        private bool isDeveloper;

        public Customer(string firstName, string lastName, bool isDeveloper = false)
        {
            FirstName = firstName;
            LastName = lastName;
            IsDeveloper = isDeveloper;
        }

        public string FirstName
        {
            get => firstName; 
            set
            {
                firstName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public string LastName
        {
            get => lastName; 
            set
            {
                lastName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public bool IsDeveloper
        {
            get => isDeveloper; 
            set
            {
                isDeveloper = value;
                OnPropertyChanged();
            }
        }

        public string DisplayName { get => FirstName + " " + LastName; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}