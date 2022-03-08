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
        Task Create(T Model);

        Task<T> Read(ID id);
        Task<List<T>> ReadAll();

        Task Update(T Model);

        Task Delete(T Model);

        Task DeleteAll();

        
    }
}
