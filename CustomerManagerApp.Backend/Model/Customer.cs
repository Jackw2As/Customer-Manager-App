using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Model
{
    public class Customer
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
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
            }
        }

        public bool IsDeveloper
        {
            get => isDeveloper;
            set
            {
                isDeveloper = value;
            }
        }

        public string DisplayName { get => FirstName + " " + LastName; }

    }
}