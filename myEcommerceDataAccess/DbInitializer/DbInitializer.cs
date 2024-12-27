using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myEcommerceDataAccess.Data;
using myEcommerceEntities.Models;
using myEcommerceUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace myEcommerceDataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        // in this class i will initialize the system migration to make the system directly 
        // implement the migration in the database if it haven't implemented yet 
        // and then i will initializer the roles in the system after the migration added to DB
        // because the DB is Empty 
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
        }

        public void Initialize()
        {
            //Migration 
            try
            {
                // this to check the count of migration that is not updated to the database 
                if (_applicationDbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _applicationDbContext.Database.Migrate();
                }

            }
            catch (Exception ex)
            {
            }
            // Roles 
            // i need to create system roles, and i will check here if is it exist in the DB or Not
            // after i create it i should go to to make the system when create identityuser create UserRole  
            //if (!_roleManager.RoleExistsAsync(STVRole.Admin).GetAwaiter().GetResult())
            //{
            //    _roleManager.CreateAsync(new IdentityRole(STVRole.Admin)).GetAwaiter().GetResult();
            //    _roleManager.CreateAsync(new IdentityRole(STVRole.Editor)).GetAwaiter().GetResult();
            //    _roleManager.CreateAsync(new IdentityRole(STVRole.Customer)).GetAwaiter().GetResult();
            //}
            // User 
           //var man= _userManager.CreateAsync(new ApplicationUser
           // {
           //     UserName = "Admin@mycommerce.com",
           //     Email = "Admin@mycommerce.com",
           //     Name = "MahmoudAdmin",
           //     PhoneNumber = "01111111111",
           //     Address = "Nazla",
           //     City = "Fayoum"
           // }, "password2000123M").GetAwaiter().GetResult();


            //ApplicationUser user = _applicationDbContext.ApplicationUsers.FirstOrDefault();
            //ApplicationUser user = new ApplicationUser();
            //user.Id = "qwwww-12234";
            //user.UserName = "Admin@mycommerce.com";
            //user.Email = "Admin@mycommerce.com";
            //user.Name = "Admin";
            //user.PhoneNumber = "1234567890";
            //user.Address = "Nazla";
            //user.City = "Fayoum";
            //IdentityUser user = new IdentityUser();
            //user.Email = "admin@mycommerce.com";
            //user.UserName = "admin@mycommerce.com";
            //user.PhoneNumber = "1234567890";
            //_userManager.CreateAsync(user, "password2000123M").GetAwaiter().GetResult();


            ////_userManager.AddToRoleAsync(user, STVRole.Admin).GetAwaiter().GetResult();




            return;

            // after i end from it i will go to the program.cs 
            // to make the system where it run execute this initializer  
        }


    }
}
