using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceEntities.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        // we should create unitofwork because we will use it to execute the all repositories and save changes in the DB by using the unit of work 
        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }

        public IApplicationUserRepository ApplicationUser { get; }

        public IShoppingCartRepository ShoppingCart { get; }
        public IOrderHeaderRepository OrderHeader { get; }
        public IOrderDetailRepository OrderDetail { get; }

        int Complete();

    }
}
