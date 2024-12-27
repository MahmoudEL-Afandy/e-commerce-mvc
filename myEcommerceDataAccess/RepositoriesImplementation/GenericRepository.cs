using Microsoft.EntityFrameworkCore;
using myEcommerceDataAccess.Data;
using myEcommerceEntities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceDataAccess.RepositoriesImplementation
{

    // i create this class to implement IGenericRepository 
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // i should add applicationDbContext to connect to the DB 
        private readonly ApplicationDbContext _context;
        // i add it to make the system add new table in the DB
        private DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
            {
                //query = query.Where(predicate);
                query = query.Where(predicate); 
            }
            if (IncludeWord != null)
            {
                foreach(var item in IncludeWord.Split( new char[] {','},StringSplitOptions.RemoveEmptyEntries ))
                {

                    query = query.Include(item);

                }

            }
            return query.ToList();

        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? predicate, string? IncludeWord = null)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (IncludeWord != null)
            {
                foreach (var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {

                    query = query.Include(item);

                }

            }
            return query.SingleOrDefault();

        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
