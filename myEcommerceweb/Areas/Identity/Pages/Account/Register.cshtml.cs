// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using myEcommerceEntities.Models;
using myEcommerceUtilities;

namespace myEcommerceweb.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        // i create role in my system so i shuold inject the role manager in the register cs 
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// my Comment : this model for the property that the user will input it to register it's own account, and if i want to add more inputs 
        /// i should its property here to can use it in register view and make it display to the user .
        /// also it should exist in the applicationUser model to make it in the database 
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            // the property (in ApplicationUser Model) that i want from the user to enter it to register the account .

            [Required]
            public string Name { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            // i need to create system roles, and i will check here if is it exist in the DB or Not
            // after i create it i should go to to make the system when create identityuser create UserRole  
            if (!_roleManager.RoleExistsAsync(STVRole.Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(STVRole.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(STVRole.Editor)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(STVRole.Customer)).GetAwaiter().GetResult();

                var user = CreateUser();

                await _userStore.SetUserNameAsync(user,"admin@myecommerce.com", CancellationToken.None);
                await _emailStore.SetEmailAsync(user, "admin@myecommerce.com", CancellationToken.None);
                user.Name = "Mahmoud";
                user.City = "Fayoum";
                user.Address = "City";
                var result = await _userManager.CreateAsync(user, "1234@Mahmoud");
                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, STVRole.Admin);
                }


            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // i should go to for this method in this file and change it from create user with type
                // identityUser to applicationUser 
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.Name = Input.Name;
                user.City = Input.City;
                user.Address = Input.Address;
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // i must add it with one of the roles in the system to give the user that is register account it's own role in system 
                    // this after i make the user create new user and make it choose the role of the user 
                    // that he will create 
                    // Form["RoleRadio"] = in radio name =  "RoleRadio" ;
                    string roleAdminEnteredInForm = HttpContext.Request.Form["RoleRadio"].ToString();
                    // roleAdminEnteredInForm if that = null or empty does it mean the user is register new account
                    // if else does it mean the admin is register new account
                    if (String.IsNullOrEmpty(roleAdminEnteredInForm))
                    {
                        await _userManager.AddToRoleAsync(user, STVRole.Customer);

                        //these will make the user after regitered new account redirct to /customer/home/index and logged in to the new account
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, roleAdminEnteredInForm);
                    }
                    //these will make the admin after regitered new account redirct to /admin/users/index and still logged in the admin account
                    return RedirectToAction("Index", "Users", new {area="Admin"});




                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {  // this will make the user after register the account redirectdirectly to /customer/home/index and logged in to new account
                        // and i need to check the new account that registered by who , and i need
                        // if user registered i want redirect to /customer/home/index and logged in to new account
                        // if the admin registered i want redirect to admin/users/index and still logged in the admin account
                        // and these two conditions responsible about it 
                        // so i need to check the user that register new account and i checked about it when i defined the role above 
                        // so i cut it from here to condition in the above 
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        //return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        // this before changes to AppUser 
        //private IdentityUser CreateUser()
        //{
        //    try
        //    {
        //        return Activator.CreateInstance<IdentityUser>();
        //    }
        //    catch
        //    {
        //        throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
        //            $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
        //            $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        //    }
        //}
        // after changes to Appuser
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
