using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Entities
{
    public class BaseEntity<T> where T : class
    {
        public T ID { get; init; }

        public BaseEntity(T id)
        {
           ID = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj != null && obj.GetType() == GetType())
            {
                BaseEntity<T> Othercustomer = (BaseEntity<T>)obj;
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