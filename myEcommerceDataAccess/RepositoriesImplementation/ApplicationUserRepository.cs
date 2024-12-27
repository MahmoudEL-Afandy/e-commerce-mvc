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
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>,IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ApplicationUser applicationUser)
        {
             _context.Update(applicationUser);
        }
    }
}
