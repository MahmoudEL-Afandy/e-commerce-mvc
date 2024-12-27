using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using myEcommerceEntities.Repositories;
using myEcommerceUtilities;
using System.Security.Claims;


namespace myEcommerceweb.Areas.Admin.Controllers
{
    
    [Area("Admin")]
    [Authorize(Roles =STVRole.Admin)]
    public class UsersController : Controller
    {
        // i create this controller to make the admin can control the users  
        public readonly IUnitOfWork _unitOfWork;
        
        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // this method will return list of users  except the user that are logged in (Admin)
        [HttpGet]
        public IActionResult GetData()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;

            var users = _unitOfWork.ApplicationUser.GetAll(x=>x.Id!=userId).AsEnumerable();
          

            return Json(new {data=users});
        }

        /* in this controller i will make delete by another way in category here i will make the system implement the sweet alert2  from this website : https://sweetalert2.github.io/
        * in this way we will make the system delete the item without go to the delete view , the delete will be with the toaster 
        * so in the controller we not need to Get method the delete the item 
        * 
        * 
        * 
        */


        [HttpDelete]
        public IActionResult Delete(string id)
        {

            var USerInDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id==id);//u => u.Id == id // Id of user in DB isn't string 

            if (USerInDb == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });

            }
            //var oldImage = Path.Combine(_webHostEnvironment.WebRootPath, productInDb.Img.TrimStart('\\'));
            //if (System.IO.File.Exists(oldImage))
            //{
            //    System.IO.File.Delete(oldImage);
            //}

            _unitOfWork.ApplicationUser.Remove(USerInDb);
            _unitOfWork.Complete();
            // TempData["delete"] = "Item Has Deleted Successfully";

            return Json(new { success = true, message = "Item Has Been Deleted Successfully " });




        }
        [HttpPost]
        public IActionResult LockUnlock(string id)
        {
            var UserInDB = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id  == id);
            if (UserInDB == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });

            }

            if (UserInDB.LockoutEnd == null || UserInDB.LockoutEnd < DateTime.Now)
            {
                UserInDB.LockoutEnd = DateTime.Now.AddYears(1);
                _unitOfWork.Complete();

                return Json(new { success = true, message = "Item Has Been Locked Successfully " });

            }
            else
            {
                UserInDB.LockoutEnd = DateTime.Now;
                _unitOfWork.Complete();
                return Json(new { success = true, message = "Item Has Been UnLocked Successfully " });

            }

          //  _unitOfWork.Complete();
          //  return RedirectToAction("Index", "Users", new { area = "Admin" });



        }
    }
}
