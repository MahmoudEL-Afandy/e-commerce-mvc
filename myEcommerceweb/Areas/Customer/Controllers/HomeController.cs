using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myEcommerceEntities.Models;
using myEcommerceEntities.Repositories;
using myEcommerceUtilities;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace myEcommerceweb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // i will create the pagination pages in Asp.net Mvc for the products in the system 
        // the paination pages can created by many of ways in the microsoft asp.net MVC documentaion or download packages x.pagedlist.MVC.Core in the project
        // and then implemeted it the project 
        [HttpGet]
        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 12;
            // i will create the following after i add the packages x.pagedlist.MVC.Core in the project

            var products = _unitOfWork.Product.GetAll().ToPagedList(pageNumber, pageSize);
            return View(products);
        }

        // this to get the page of product details and make the user can add item to cart 
        //[HttpGet]
        public IActionResult Details(int id)
        {
            // i will create view model  called Shopping cart contains Product , Count , and i create it to get the count of the product the user want to add it in cart. 
           // var productId = id;
            var productInDb = _unitOfWork.Product.GetFirstOrDefault(x=>x.Id== id, IncludeWord:"Category");
            if (productInDb == null)
            {
                NotFound();
            }
            ShoppingCart shoppingCartVM = new ShoppingCart()
            {
                ProductId = id,
                Product = productInDb,
                Count = 1
            };
            return View(shoppingCartVM);

          //  return View(productInDb);

        }
        // i create it to save the items that the user choose it to buy and i should make it authorize to make the system 
        // can save details about the user that add the item to cart 

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {

            
           

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var shoppingCartInDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.ApplicationUserId == claim.Value && x.ProductId==shoppingCart.ProductId);

            if (shoppingCartInDb == null)
            {
                var shoppingCartItem = new ShoppingCart();
                shoppingCartItem.Count = shoppingCart.Count;
                shoppingCartItem.ProductId = shoppingCart.ProductId;

                shoppingCartItem.ApplicationUserId = claim.Value;


                _unitOfWork.ShoppingCart.Add(shoppingCartItem);
                _unitOfWork.Complete();
                // After i add ShoppingCart In Db i Need to add it in the session

                HttpContext.Session.SetInt32(STVRole.SessionKey,_unitOfWork.ShoppingCart.GetAll(x=>x.ApplicationUserId==claim.Value).Count());
                // to make the system change number of cart in the session as in the data base when user add or remove item from cart
                // we should reflect this change in the session 

            }
            else
            {
                _unitOfWork.ShoppingCart.IncreaseCount(shoppingCartInDb, shoppingCart.Count);
                _unitOfWork.Complete();
            }





            return RedirectToAction("Index");
        }


        
        


    }
}
