using myEcommerceDataAccess.Data;
using myEcommerceEntities.Models;
using myEcommerceEntities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceDataAccess.RepositoriesImplementation
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {

            _context = context;
        }

        public int DecreaseCount(ShoppingCart cart, int newCount)
        {
               cart.Count -= newCount;
               return cart.Count;
        }

        public int IncreaseCount(ShoppingCart cart, int newCount)
        {
            cart.Count += newCount;
            return cart.Count;
        }

        
    }
}
