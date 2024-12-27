using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceEntities.Repositories
{
    //this interface we shuld create it to create the generic method that are use in the most models 
    // we create the repository pattern to create layer between the DB(DbContext) and controller of each model 
    // we will create specific interface for each model in the system and make it implement the IGenericRepository and then add it's own method;
    public interface IGenericRepository<T> where T : class
    {
        //_context.categories.include("products").ToList();
        //_context.categories.where(x=>x.name==name).ToList();
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null);

        //_context.categories.include("products").ToSingleOrDefault();
        //_context.categories.where(x=>x.name==name).ToSingleOrDefault();
        T GetFirstOrDefault(Expression<Func<T, bool>>? predicate, string? IncludeWord = null);
        //_context.categories.add(category);
        void Add(T entity);
        //_context.categories.remove(category);
        void Remove (T entity);

        void RemoveRange(IEnumerable<T> entities);


    }
}
