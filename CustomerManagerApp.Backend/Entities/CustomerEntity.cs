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
    public class CustomerEntity
    {
        [JsonConstructor]
        public CustomerEntity(string firstName, string lastName, string DrinkId, bool isDeveloper = false, string? id = null)
        {
            FirstName = firstName;
            LastName = lastName;
            IsDeveloper = isDeveloper;
            if (String.IsNullOrWhiteSpace(id)) { Id = Guid.NewGuid().ToString(); }
            else { Id = id; }
            DrinkID = DrinkId;
        }
        public CustomerEntity(CustomerEntity customer)
        {
            DrinkID = customer.DrinkID;
            FirstTime = customer.FirstTime;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            IsDeveloper = customer.IsDeveloper;
            if (String.IsNullOrWhiteSpace(customer.Id)) { Id = Guid.NewGuid().ToString(); }
            else { Id = customer.Id; }
        }
        //ID is a Guid. The Reason we return a string instead of GUID object
        // is because the Json Seralizer doesn't work with GUID objects.
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
                CustomerEntity Othercustomer = (CustomerEntity)obj;
                return Id == Othercustomer.Id;
            }
            return base.Equals(obj);
        }

        //Not great, but at the end of the day the only thing that really matters here is the Id of the entity.
        //If they match then they are the same value regaurdless of other data.
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}