using Microsoft.AspNetCore.Mvc;
using myEcommerceEntities.Repositories;
using myEcommerceUtilities;
using System.Security.Claims;

namespace myEcommerceweb.ViewComponents
{
    public class ShoppingCartViewComponent:ViewComponent
    {
        // the second way for create and destroyed session, this way in course and this way to learn how create ViewCompnent 
        // the view component is similar to partial view   
        // we use view component to create component and share it between views by invoke the component the view that we want to display in it .


        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       

        public async Task<IViewComponentResult> InvokeAsync ()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if ( claim != null)
            {
                if (HttpContext.Session.GetInt32(STVRole.SessionKey) != null)
                {
                    return View (HttpContext.Session.GetInt32 (STVRole.SessionKey));
                }
                else
                {
                    HttpContext.Session.SetInt32(STVRole.SessionKey, _unitOfWork.ShoppingCart.GetAll(x=>x.ApplicationUserId==claim.Value).Count());
                    return View(HttpContext.Session.GetInt32(STVRole.SessionKey));
                }
                
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }


        }
    }
}
