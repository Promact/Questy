using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promact.Trappist.DomainModel.ApplicationClasses.Account;
using Promact.Trappist.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public object SignInStatus { get; private set; }

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory)

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
        /// <returns>Invalid login attempt</returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(Login model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            { }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            return View(model);
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
