using Khosomat.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Khosomat.DataAccess.Repos
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context  = context;
            _dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, string? IncludeTable = null)
        {
            // _context.Categories
            IQueryable<T> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);//_context.Categories.Where(x => x.blablabla)

            if(IncludeTable != null)
            {
                //_context.Categories.Where(x => x.blablabla).Include("Products").Include(...
                foreach (var item in IncludeTable.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();

        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? predicate = null, string? IncludeTable = null)
        {
            // _context.Categories
            IQueryable<T> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);//_context.Categories.Where(x => x.blablabla)

            if (IncludeTable != null)
            {
                //_context.Categories.Where(x => x.blablabla).Include("Products").Include(...
                foreach (var item in IncludeTable.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.SingleOrDefault();
        }
    }
}
