using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Entities
{
    public class CustomerEntity : BaseEntity<string>
    {
        [JsonConstructor]
        public CustomerEntity(string id, string firstName, string lastName, string drinkId, DateTimeOffset firstTime, bool isDeveloper = false) : base (id)
        {
            FirstName = firstName;
            LastName = lastName;
            IsDeveloper = isDeveloper;
            DrinkID = drinkId;
            FirstTime = firstTime;
        }
        public CustomerEntity(CustomerEntity customer) : base(customer.ID)
        {
            DrinkID = customer.DrinkID;
            FirstTime = customer.FirstTime;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            IsDeveloper = customer.IsDeveloper;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeveloper { get; set; }
        public DateTimeOffset FirstTime { get; set; }
        public string DrinkID { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj != null && obj.GetType() == GetType())
            {
                CustomerEntity Othercustomer = (CustomerEntity)obj;
                return ID == Othercustomer.ID;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}