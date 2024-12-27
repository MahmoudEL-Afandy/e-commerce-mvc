using Microsoft.EntityFrameworkCore;
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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) :base(context) 
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var CategoryInDB = _context.Categories.FirstOrDefault(x=> x.Id == category.Id);
            if (CategoryInDB != null)
            {
                CategoryInDB.Name = category.Name;
                CategoryInDB.Description = category.Description;
                

            }
           
        }
    }
}
