using myEcommerceEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceEntities.Repositories
{
    public interface IShoppingCartRepository:IGenericRepository<ShoppingCart>
    {
        // i need to create this method to make the user can  edit the count item 
        // and create two method and not cretae one update method because i want update only the Count 
        int DecreaseCount (ShoppingCart cart , int newCount);
        int IncreaseCount (ShoppingCart cart , int newCount);

    }
}
