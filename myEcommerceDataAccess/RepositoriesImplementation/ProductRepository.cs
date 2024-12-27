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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }
        public void UpDate(Product product)
        {
            var productInDB = _context.Products.FirstOrDefault(x=>x.Id == product.Id);
            if (productInDB != null)
            {
                productInDB.Name = product.Name;
                productInDB.Description = product.Description;
                productInDB.Price = product.Price;
                productInDB.Img = product.Img;
                productInDB.CategoryId = product.CategoryId;
            }
               
        }
    }
}
