using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myEcommerceEntities.Models;
using myEcommerceEntities.Repositories;
using myEcommerceEntities.ViewModels;

namespace myEcommerceweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment; // i add this to define the place that the system will save the image 
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
          
            return View();
        }
        [HttpGet]
        public IActionResult GetData()
        {
            var products = _unitOfWork.Product.GetAll(IncludeWord:"Category");

            return Json(new {data =products} );
        }

        // create 

        [HttpGet]
        public IActionResult Create()
        {
            // we should create ProductVM to return it to the create View 
            ProductVM productsVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x=> new SelectListItem
                {
                    // text that will display to select in the create view and the user will select one of them Text will be the name of the category
                    // Value : will be the value that will be input and will saved in the DB and must be string so we will convert it to string 
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(productsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM productVM , IFormFile file) // we should add iformfile to make the system can add image and save it in the database and iFormFile variables should be the same as in input name  
        {

            if (ModelState.IsValid)
            {
                var RootPath = _webHostEnvironment.WebRootPath; // this to arrive the path of the wwwroot 
                if(file!=null)
                {
                    string fileName = Guid.NewGuid().ToString(); // this to create unique random name to image to can get it from the Database
                    var fullPath = Path.Combine(RootPath, @"images\Products");
                    // we should know type of the file extension 
                    var fileExtension = Path.GetExtension(file.FileName);
                    // any media we should stream it to can save it 
                    using (var fileStream = new FileStream(Path.Combine(fullPath, fileName + fileExtension),FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                    }
                    // after all these steps i saved the image in the DB and  we can get it again and use 
                    // now we will save only the URL of the image in the data base 

                    productVM.Product.Img = @"images\Products\" + fileName + fileExtension;
                }
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Complete();
                TempData["create"] = "Data Has Created Successfully";

                return RedirectToAction("Index");
            }
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            return View(productVM);


        }

        [HttpGet]
        public IActionResult Edit(int? id) 
        {
            if (id == null || id == 0) return NotFound();
            var product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);

            ProductVM productVm = new ProductVM()
            {
                Product = product,
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(productVm);



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (ProductVM productVM , IFormFile? file) // iFormFile should be nullable here  if the user want to let it without update 
        {
            if (ModelState.IsValid)
            {
                var RootPath = _webHostEnvironment.WebRootPath; // this to arrive the path of the wwwroot 

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString(); // this to create unique random name to image to can get it from the Database
                    var fullPath = Path.Combine(RootPath, @"images\Products");
                    // we should know type of the file extension 
                    var fileExtension = Path.GetExtension(file.FileName);
                    // this for delete old image from the system 
                    if (productVM.Product.Img!=null)
                    {
                        var oldimg = Path.Combine(RootPath,productVM.Product.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }

                    }
                    // any media we should stream it to can save it 
                    using (var fileStream = new FileStream(Path.Combine(fullPath, fileName + fileExtension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                    }
                    // after all these steps i saved the image in the DB and  we can get it again and use 
                    // now we will save only the URL of the image in the data base 
                    productVM.Product.Img = @"images\Products\" + fileName + fileExtension;
                }


                _unitOfWork.Product.UpDate(productVM.Product);
                _unitOfWork.Complete();
                TempData["update"] = "Data Has updated Successfully";
                return RedirectToAction("Index");


            }
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            return View(productVM);
        }


        /* in this controller i will make delete by another way in category here i will make the system implement the sweet alert2  from this website : https://sweetalert2.github.io/
         * in this way we will make the system delete the item without go to the delete view , the delete will be with the toaster 
         * so in the controller we not need to Get method the delete the item 
         * 
         * 
         * 
         */


        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var productInDb = _unitOfWork.Product.GetFirstOrDefault(x=>x.Id==id);

            if (productInDb == null)
            {
                return Json(new {success = false, message = "Error While Deleting"});

            }
            var oldImage = Path.Combine(_webHostEnvironment.WebRootPath,productInDb.Img.TrimStart('\\'));
            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }

            _unitOfWork.Product.Remove(productInDb);
            _unitOfWork.Complete();
            // TempData["delete"] = "Item Has Deleted Successfully";

            return Json(new { success = true, message = "Item Has Been Deleted Successfully " });




        }
    
    
    
    
    }
}
