using myEcommerceDataAccess.Data;
using myEcommerceEntities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceDataAccess.RepositoriesImplementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Category {  get; private set; }

        public IProductRepository Product { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public IShoppingCartRepository ShoppingCart {  get; private set; }

		public IOrderHeaderRepository OrderHeader { get; private set; }

		public IOrderDetailRepository OrderDetail { get; private set; }

		public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);
            ShoppingCart = new ShoppingCartRepository(context);
            OrderHeader = new OrderHeaderRepository(context);
            OrderDetail = new OrderDetailRepository(context);

            
        }

        public int Complete()
        {

            return _context.SaveChanges();
           
        }

        public void Dispose()
        {
           _context.Dispose();
        }
    }
}
