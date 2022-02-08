using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Entities
{
    public class DrinkEntity : BaseEntity<string>
    {
        public DrinkEntity(string name) : base(name)
        {
            Name = name;
        }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            DrinkEntity? other = obj as DrinkEntity;
            if (other == null) return false;
            if (ID == other.ID) return true;
            else { return false; }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
