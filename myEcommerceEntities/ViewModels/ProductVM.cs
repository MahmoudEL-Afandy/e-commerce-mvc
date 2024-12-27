using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using myEcommerceEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myEcommerceEntities.ViewModels
{
    public class ProductVM
    {
        // we should create this view model to can get list of caegory from the DB to make the user select one ad then add the id in the DB 

        public Product Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
