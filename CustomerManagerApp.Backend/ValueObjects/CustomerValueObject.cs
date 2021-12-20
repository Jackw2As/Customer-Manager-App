using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.ValueObjects
{
    public class CustomerValueObject
    {
        [JsonConstructor]
        public CustomerValueObject(string firstName, string lastName, string DrinkId, bool isDeveloper = false, string? id = null)
        {
            FirstName = firstName;
            LastName = lastName;
            IsDeveloper = isDeveloper;
            if (id == null) { Id = Guid.NewGuid().ToString(); }
            else { Id = id; }
            DrinkID = DrinkId;
        }
        public CustomerValueObject(CustomerValueObject customer)
        {
            DrinkID = customer.DrinkID;
            FirstTime = customer.FirstTime;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            IsDeveloper = customer.IsDeveloper;
            Id = new Guid().ToString();
        }
        //ID is a Guid. The Reason we return a string instead of GUID object
        // is because the Json Seralizer doesn't work well with GUID objects.
        public string Id { get; init; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeveloper { get; set; }
        public DateTimeOffset FirstTime { get; set; }
        public string DrinkID { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj != null && obj.GetType() == GetType())
            {
                CustomerValueObject Othercustomer = (CustomerValueObject)obj;
                return Id == Othercustomer.Id;
            }
            return base.Equals(obj);
        }
    }
}