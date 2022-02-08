using CustomerManagerApp.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository
{
    public interface IRepository<T, ID> where T : BaseEntity<ID> where ID : class
    {
        void Create(T Model);

        T Read(ID id);
        List<T> ReadAll();

        void Update(T Model);

        void Delete(T Model);

        void DeleteAll();

        
    }
}
