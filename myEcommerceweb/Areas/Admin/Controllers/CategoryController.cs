using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myEcommerceDataAccess.Data;
using myEcommerceEntities.Models;
using myEcommerceEntities.Repositories;

namespace myEcommerceweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;

        /*
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
            
        }
        */
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {

            //var categories = _context.Categories.ToList();
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        //CSRF:Cross Side Forgery Attacks
        /*Preventing CSRF Attacks in ASP.NET MVC:
            ASP.NET MVC provides built-in mechanisms to protect against CSRF attacks. Here are some common approaches to prevent CSRF attacks in ASP.NET MVC:

            Anti-CSRF Tokens:
                Use the ValidateAntiForgeryToken attribute on your controller actions to ensure that a request is valid and originates from your site.
         */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {

            if (ModelState.IsValid)//ServerSide Validation Error
            {
                /*
                _context.Categories.Add(category);
                _context.SaveChanges();
                */
                _unitOfWork.Category.Add(category);
                _unitOfWork.Complete();
                TempData["create"] = "Data Has Created Successfully";
                return RedirectToAction("Index");
            }
            return View(category);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                NotFound();
            }
            //  var category = _context.Categories.Find(id);

            var category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                // _context.Categories.Update(category);
                // _context.SaveChanges();
                _unitOfWork.Category.Update(category);
                _unitOfWork.Complete();
                TempData["update"] = "Data Has updated Successfully";
                return RedirectToAction("Index");


            }

            return View(category);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                NotFound();
            }
            //  var category = _context.Categories.Find(id);
            var category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            return View(category);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int? id)
        {
            // var category = _context.Categories.Find(id);
            var category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                NotFound();
            }
            // _context.Categories.Remove(category);
            // _context.SaveChanges();
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Complete();
            TempData["delete"] = "Data has deleted successfully";
            return RedirectToAction("Index");
        }






    }
}
