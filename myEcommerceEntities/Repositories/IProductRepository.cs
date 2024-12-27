using myEcommerceEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceEntities.Repositories
{
    public interface IProductRepository: IGenericRepository<Product>
    {

        void UpDate(Product product);

    }
}
