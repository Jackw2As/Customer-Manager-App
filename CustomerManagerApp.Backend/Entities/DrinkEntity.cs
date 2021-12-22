using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Entities
{
    public class DrinkEntity
    {
        public DrinkEntity(string name)
        {
            Name = name;

            //If A job has the same name as another job its the same job
            // as far as this program is concerned.
            // This is only because I want to use hardcoded Drink values
            // rather then importing them from a perisetent storage
            // otherwise id use a GUID.
            Id = name;
        }
        public string Id { get; init; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            DrinkEntity? other = obj as DrinkEntity;
            if (other == null) return false;
            if (Id == other.Id) return true;
            else { return false; }
        }

        //Not great, but at the end of the day the only thing that really matters here is the Id of the entity.
        //If they match then they are the same value regaurdless of other data.
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
