using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myEcommerceEntities.Models;
using myEcommerceEntities.Repositories;
using myEcommerceEntities.ViewModels;
using myEcommerceUtilities;
using Stripe;

namespace myEcommerceweb.Areas.Admin.Controllers
{
    [Area(STVRole.Admin)]
    [Authorize(Roles =STVRole.Admin)]   
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        public OrderController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetData()
        {
            // this method for getdata for  display order in order.js file 
            // and it will return list of orderheader that contains all data about the order 

            var ordersHeadersInDB = _unitOfWork.OrderHeader.GetAll(IncludeWord: "ApplicationUser");
            return Json(new {data = ordersHeadersInDB});

        }

        [HttpGet]
        public IActionResult Details(int orderHId)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x=>x.Id == orderHId , IncludeWord:"ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetail.GetAll(x=>x.OrderHeaderId ==orderHId,IncludeWord:"Product")
            };
            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpDateOrder (OrderHeader orderHeader)
        {

            var OrHeaderInDb= _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == orderHeader.Id);

            OrHeaderInDb.Name = orderHeader.Name;
            OrHeaderInDb.Phone = orderHeader.Phone;
            OrHeaderInDb.Address = orderHeader.Address;
            OrHeaderInDb.City = orderHeader.City;
            if (orderHeader.Carrier!=null)
            {
                OrHeaderInDb.Carrier = orderHeader.Carrier;
            }
            if (orderHeader.TrackingNumber!=null)
            {
                OrHeaderInDb.TrackingNumber = orderHeader.TrackingNumber;
            }



            _unitOfWork.OrderHeader.UpDate(OrHeaderInDb);
            _unitOfWork.Complete();
            TempData["update"] = "Data Has updated Successfully";
            return RedirectToAction("Details", "Order", new { orderHId = OrHeaderInDb.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcess (OrderHeader orderHeader)
        {

            _unitOfWork.OrderHeader.UpDateOrderStatus(orderHeader.Id, STVRole.Processing,null);

            _unitOfWork.Complete();
            TempData["update"] = "Order Status Has updated Successfully";
            return RedirectToAction("Details", "Order", new { orderHId = orderHeader.Id });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartShipping(OrderHeader orderHeader)
        {

            var OrderInDb = _unitOfWork.OrderHeader.GetFirstOrDefault(x=>x.Id==orderHeader.Id);

            OrderInDb.OrderStatus = STVRole.Shipped;
            OrderInDb.TrackingNumber = orderHeader.TrackingNumber;
            OrderInDb.Carrier = orderHeader.Carrier;
            OrderInDb.ShippingDate = DateTime.Now;


            _unitOfWork.OrderHeader.UpDate(OrderInDb);
            _unitOfWork.Complete();
            TempData["update"] = "Order Has Shipped Successfully";
            return RedirectToAction("Details", "Order", new { orderHId = orderHeader.Id });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder(OrderHeader orderHeader)
        {
            //var orderInDb = _unitOfWork.OrderHeader.GetFirstOrDefault(x=>x.Id == orderHeader.Id);

            // i create it to check if the order was approved , we should make stripe refund the order 
            if (orderHeader.PaymentStatus == STVRole.Approved)
            {
                // all this for implement stripe refund approved order 
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PayIntentId


                };
                var service = new RefundService();
                Refund refund = service.Create(option);
                _unitOfWork.OrderHeader.UpDateOrderStatus(orderHeader.Id, STVRole.Cancelled, STVRole.Refund);

            }
            else
            {
                _unitOfWork.OrderHeader.UpDateOrderStatus(orderHeader.Id, STVRole.Cancelled, STVRole.Cancelled);

            }
            _unitOfWork.Complete();
            TempData["update"] = "Order Has Cancelled Successfully";
            return RedirectToAction("Details", "Order", new { orderHId = orderHeader.Id });



        }

    }
}
