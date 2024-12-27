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
    public class Product
    {
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
        
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }

        [ValidateNever]
        [DisplayName("Image")]
        public string Img { get; set; }
        [DisplayName("Category")]
        [Required]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        

    }
}
