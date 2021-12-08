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
        public Guid id { get; set; }

        public Customer(string firstName, string lastName, bool isDeveloper = false, Guid guid = new())
        {
            this.firstName = firstName;
            this.lastName = lastName;
            IsDeveloper = isDeveloper;
            id = guid;
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

        public override bool Equals(object? obj)
        {
            if (obj != null && obj.GetType() == this.GetType())
            {
                Customer Othercustomer = (Customer)obj;
                return this.id == Othercustomer.id;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
          return base.GetHashCode();
        }
    }
}