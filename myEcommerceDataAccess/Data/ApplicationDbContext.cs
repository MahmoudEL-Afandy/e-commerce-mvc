using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myEcommerceEntities.Models;


namespace myEcommerceDataAccess.Data
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>            //DbContext i should change it to identityDbContext to can work with user identity 
    {
        //DbContext i should change it to identityDbContext to can work with user identity because identityDbContext have tables specilize in identity 

        public ApplicationDbContext( DbContextOptions<ApplicationDbContext>options):base(options) 
        {
            
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

         // i should add it for Scafoling items and not mapped to database 
         public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<OrderHeader> OrderHeaders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
