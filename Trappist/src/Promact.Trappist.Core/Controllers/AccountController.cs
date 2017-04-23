using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Account;
using Promact.Trappist.Repository.Account;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IStringConstants _stringConstant;
        public AccountController(IAccountRepository accountRepository, IStringConstants stringConstant)
        {
            _accountRepository = accountRepository;
            _stringConstant = stringConstant;
        }
        /// <summary>
        /// this method is used to see the view of login
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
        ///  This method will be called with credentials to validate user
        /// </summary>
        /// <returns>succesful login-checking for user and redirect to testdashboard page</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login loginModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (await _accountRepository.SignIn(loginModel))
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ViewBag.Error = _stringConstant.InavalidLoginError;
                    return View(loginModel);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, _stringConstant.InavalidModelError);
                return View(loginModel);
            }
        }
        /// <summary>
        /// this method is used to redirect to any local url link
        /// </summary>
        /// <param name="returnUrl">string type of url</param>
        /// <returns></returns>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
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
