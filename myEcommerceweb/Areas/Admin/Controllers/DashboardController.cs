using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myEcommerceEntities.Repositories;
using myEcommerceUtilities;


namespace myEcommerceweb.Areas.Admin.Controllers
{
    [Area(STVRole.Admin)]
    [Authorize(Roles =STVRole.Admin)]   
    public class DashboardController : Controller
    {
        public readonly IUnitOfWork _uniteOfWork;
        public DashboardController(IUnitOfWork uniteOfWork) 
        {
            _uniteOfWork = uniteOfWork;
        }

        public IActionResult Index()
        {
            // i will create View Bag to Get some values to display it in Admin Dashboard index page .
            ViewBag.AllOrders = _uniteOfWork.OrderHeader.GetAll().Count();
            ViewBag.ApprovedOrders = _uniteOfWork.OrderHeader.GetAll(x=>x.OrderStatus==STVRole.Approved).Count();
            ViewBag.AllUsers = _uniteOfWork.ApplicationUser.GetAll().Count();
            ViewBag.AllCategories = _uniteOfWork.Category.GetAll().Count();


            return View();
        }
    }
}
