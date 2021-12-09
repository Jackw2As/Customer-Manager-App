﻿using System;
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
        public Customer(string firstName, string lastName, string DrinkId, bool isDeveloper = false, string? id = null)
        {
            FirstName = firstName;
            LastName = lastName;
            IsDeveloper = isDeveloper;
            if (id == null) {   Id = Guid.NewGuid().ToString(); }
            else { Id = (id); }
            DrinkID = DrinkId;

        }
        //ID is a Guid. The Reason we return a string instead of GUID object
        // is because the Json Seralizer doesn't work well with GUID objects.
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeveloper { get; set; }
        public DateTimeOffset FirstTime { get; set; }
        public string DrinkID { get; set; }
        

        public override bool Equals(object? obj)
        {
            if (obj != null && obj.GetType() == this.GetType())
            {
                Customer Othercustomer = (Customer)obj;
                return Id == Othercustomer.Id;
            }
            return base.Equals(obj);
        }
    }
}