using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiredBrainCoffee_Customer_Manager_App.Model
{
    public class Customer
    {
        public Customer(string firstName, string lastName, bool isDeveloper = false)
        {
            FirstName = firstName;
            LastName = lastName;
            IsDeveloper = isDeveloper;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsDeveloper { get; set; }

        public string DisplayName { get => FirstName + " " + LastName; }
    }
}
