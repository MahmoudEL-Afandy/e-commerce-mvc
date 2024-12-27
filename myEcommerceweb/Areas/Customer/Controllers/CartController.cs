using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using myEcommerceEntities.Models;
using myEcommerceEntities.Repositories;
using myEcommerceEntities.ViewModels;
using myEcommerceUtilities;
using Stripe.Checkout;
using System.Security.Claims;

namespace myEcommerceweb.Areas.Customer.Controllers
{
    [Area(STVRole.Customer)]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public ShoppingCartVM ShoppingCartVM { get; set; }
		[HttpGet]
		public IActionResult Index()
       {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            ShoppingCartVM = new ShoppingCartVM()
            {
                CartsList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == userId, IncludeWord: "Product")


            };

            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.CartsTotalPrice += item.Count * item.Product.Price;
            }


			return View(ShoppingCartVM);
        }

        [HttpGet]
        public IActionResult Plus(int cartId)
        {

            var shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(x=>x.Id == cartId);

            _unitOfWork.ShoppingCart.IncreaseCount(shoppingCart, 1);
            _unitOfWork.Complete();

            return RedirectToAction("Index1");

        }

        [HttpGet]
        public IActionResult Minus(int cartId)
        {

            var shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(x=>x.Id==cartId); 


            if (shoppingCart.Count<= 1)
            {
                // in this case when count of ShoppingCart <= 1 remove this cart 
                _unitOfWork.ShoppingCart.Remove(shoppingCart);
                _unitOfWork.Complete();
                // after the cart removed from the data base i need it reflect in the system so i need to add it in my session 
                var CartsCount = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == shoppingCart.ApplicationUserId).Count();
                HttpContext.Session.SetInt32(STVRole.SessionKey, CartsCount);



            }
            else
            {
                _unitOfWork.ShoppingCart.DecreaseCount(shoppingCart, 1);
                _unitOfWork.Complete();

            }
			_unitOfWork.Complete();
            return RedirectToAction("Index1");

        }


        //[HttpDelete]
        public IActionResult Delete(int cartId)
        {
			//asp-action="Delete" asp-route-cartId="@item.Id"
			//asp-action = "Delete" must be match the method name (Delete) in the Controller & method Variable name (cartId) must match asp-route-cartId(cartId) .

			var shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);

            _unitOfWork.ShoppingCart.Remove(shoppingCart);
            _unitOfWork.Complete();
            var CartsCount = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == shoppingCart.ApplicationUserId).Count();
            HttpContext.Session.SetInt32(STVRole.SessionKey, CartsCount);

            return RedirectToAction("Index1");

        }


        [HttpGet]
        public IActionResult Summary ()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);




            ShoppingCartVM = new ShoppingCartVM()
            {
                CartsList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, IncludeWord: "Product"),
                OrderHeader = new ()
            };


            //ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == claim.Value);
            //ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            //ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.OrderHeader.TotalPrice += (item.Product.Price * item.Count);
            }


            return View(ShoppingCartVM);  
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ActionName("Summary")]
        //public IActionResult Summary (ShoppingCartVM cart)
        //{

        //    var claimIdentity = (ClaimsIdentity)User.Identity;
        //    var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

        //    cart.CartsList = _unitOfWork.ShoppingCart.GetAll(x=>x.ApplicationUserId==claim.Value, IncludeWord:"Product");

        //    var pending = STVRole.Pending;

        //    cart.OrderHeader.OrderStatus = "Pending";
        //    cart.OrderHeader.PaymentStatus = "Pending";
        //    cart.OrderHeader.OrderDate = DateTime.Now;
        //    cart.OrderHeader.ApplicationUserId = claim.Value;
        //    foreach (var item in cart.CartsList)
        //    {
        //        cart.OrderHeader.TotalPrice = (item.Product.Price*item.Count);
        //    }

        //    _unitOfWork.OrderHeader.Add(cart.OrderHeader);
        //    _unitOfWork.Complete();
        //    foreach (var item in cart.CartsList)
        //    {
        //        OrderDetail orderDetail = new OrderDetail()
        //        {
        //            ProductId = item.ProductId,
        //            OrderHeaderId=cart.OrderHeader.Id,
        //            Count = item.Count,
        //            Price = item.Product.Price


        //        };

        //        _unitOfWork.OrderDetail.Add(orderDetail);
        //        _unitOfWork.Complete();

        //    }

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult Summary(OrderHeader orderHeader)
        {

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cartsList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value, IncludeWord: "Product");

            orderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == claim.Value);
            

            orderHeader.OrderStatus = STVRole.Pending;
            orderHeader.PaymentStatus = STVRole.Pending;
            orderHeader.OrderDate = DateTime.Now;
            orderHeader.ApplicationUserId = claim.Value;
            foreach (var item in cartsList)
            {
                orderHeader.TotalPrice += (item.Product.Price * item.Count);
            }

            _unitOfWork.OrderHeader.Add(orderHeader);
            _unitOfWork.Complete();
            foreach (var item in cartsList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = orderHeader.Id,
                    Count = item.Count,
                    Price = item.Product.Price


                };

                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Complete();

            }
            // stripe implementation 
            // to make it work i should (using Stripe.Checkout;) after i download stripe.net package 
            // all the following code in this method and confirmation method from i implement it based in my business logic and 
            // and code from stripe website (https://docs.stripe.com/checkout/quickstart) to make payment feature work 

            var domain = "https://localhost:7174/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/orderconfirmation?id={orderHeader.Id}",
                CancelUrl = domain + "customer/cart/index",
            };

            foreach (var item in cartsList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        // i create it to to convert from decimal to long 
                        UnitAmount=(long)(item.Product.Price*100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item .Product.Name
                        }
                        

                    },
                    //Price = "{{PRICE_ID}}",
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            orderHeader.SessionId = session.Id;
            //orderHeader.PayIntentId = session.PaymentIntentId;
            _unitOfWork.Complete();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            ////return RedirectToAction("Index");
        }

        public IActionResult OrderConfirmation ( int id )
        {
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x=>x.Id == id );
            var service = new SessionService();
            Session session = service.Get( orderHeader.SessionId );

            if (session.PaymentStatus.ToLower()=="paid")
            {
                _unitOfWork.OrderHeader.UpDateOrderStatus(id,STVRole.Approved, STVRole.Approved);
                orderHeader.PayIntentId = session.PaymentIntentId;
                _unitOfWork.Complete();

            }

            //var claimIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cartsList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == orderHeader.ApplicationUserId);



            _unitOfWork.ShoppingCart.RemoveRange(cartsList);
            _unitOfWork.Complete();
            HttpContext.Session.Clear();
            return View(id);

        }

       public IActionResult Index1 ()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            ShoppingCartVM = new ShoppingCartVM()
            {
                CartsList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == userId, IncludeWord: "Product")


            };

            foreach (var item in ShoppingCartVM.CartsList)
            {
                ShoppingCartVM.CartsTotalPrice += item.Count * item.Product.Price;
            }


            return View(ShoppingCartVM);
        }




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult PostSummary (OrderHeader orderHeader)
        //{

        //    var claimIdentity = (ClaimsIdentity)User.Identity;
        //    var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

        //    //shoppingCartVM.CartsList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value, IncludeWord: "Product");

        //    //var pending = STVRole.Pending;

        //    //shoppingCartVM.OrderHeader.OrderStatus = "Pending";
        //    //shoppingCartVM.OrderHeader.PaymentStatus = "Pending";
        //    //shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
        //    //shoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;
        //    //foreach (var item in shoppingCartVM.CartsList)
        //    //{
        //    //    shoppingCartVM.OrderHeader.TotalPrice = (item.Product.Price * item.Count);
        //    //}

        //    //_unitOfWork.OrderHeader.Add(shoppingCartVM.OrderHeader);
        //    //_unitOfWork.Complete();
        //    //foreach (var item in shoppingCartVM.CartsList)
        //    //{
        //    //    OrderDetail orderDetail = new OrderDetail()
        //    //    {
        //    //        ProductId = item.ProductId,
        //    //        OrderHeaderId = shoppingCartVM.OrderHeader.Id,
        //    //        Count = item.Count,
        //    //        Price = item.Product.Price


        //    //    };

        //    //    _unitOfWork.OrderDetail.Add(orderDetail);
        //    //    _unitOfWork.Complete();

        //    //}

        //    return RedirectToAction("Index");

        //}
    }
}
