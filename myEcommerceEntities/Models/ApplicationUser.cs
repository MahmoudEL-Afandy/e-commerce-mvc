using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceEntities.Models
{
    // i need to add some property to identity user in DB so i should create new class and make it inherit from IdentityUser 
    // and then add Property that i want 
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        //[ValidateNever]
        //[DisplayName("Image")]
        //public string? UserImg { get; set; }

    }
}
