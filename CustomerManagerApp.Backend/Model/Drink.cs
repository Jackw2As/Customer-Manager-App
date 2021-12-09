﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Model
{
    public class Drink
    {
        public Drink(string name)
        {
            Name = name;

            //If A job has the same name as another job its the same job
            // as far as this program is concerned.
            // This is only because I want to use hardcoded Drink values
            // rather then importing them from a perisetent storage
            // otherwise id use a GUID.
            Id = name;
        }
        public string Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            Drink? other = obj as Drink;
            if (other == null) return false;
            if (Id == other.Id) return true;
            else  {   return false;   }
        }
    }
}
