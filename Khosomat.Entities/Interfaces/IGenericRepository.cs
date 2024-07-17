using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Khosomat.Entities.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        T GetFirstOrDefault(Expression<Func<T, bool>>? predicate = null, string? IncludeTable = null);
        //IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? predicate = null, string? IncludeTable = null);
    }
}
