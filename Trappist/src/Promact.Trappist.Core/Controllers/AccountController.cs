using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Account;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    public class AccountController : Controller
    {
        public ILoginRepository loginInterface { get; set; }
        public AccountController(ILoginRepository iLogin)
        {
            loginInterface = iLogin;
        }
        /// <summary>
        /// Login via Get method
        /// </summary>
        /// <returns>Login form view</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        
        /// <summary>
        /// Login via Post method
        /// </summary>
        /// <returns>succesful login-checking for user and redirect to testdashboard page</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login loginModel, string returnUrl = null)
        {           
            if (ModelState.IsValid)
            {
                if (await loginInterface.LoginUser(loginModel)) 
                {
                    return RedirectToAction("default"," ");
                }
                else
                {
                    ViewBag.Error ="UserName Or Password Invalid !";
                    return View(loginModel);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(loginModel);
            }
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
