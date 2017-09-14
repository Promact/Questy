using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Account;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using Promact.Trappist.Repository.BasicSetup;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.EmailServices;
using Promact.Trappist.Web.Controllers;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    public class AccountController : Controller
    {
        #region Private variables
        private readonly IStringConstants _stringConstant;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EmailSettings _emailSettings;
        private readonly IBasicSetupRepository _basicSetupRepository;
        #endregion

        #region Constructor
        public AccountController(IStringConstants stringConstant, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService, EmailSettings emailSettings, IBasicSetupRepository basicSetupRepository)
        {
            _stringConstant = stringConstant;
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;
            _emailSettings = emailSettings;
            _basicSetupRepository = basicSetupRepository;
        }
        #endregion

        #region Public Methods
        #region Login API
        /// <summary>
        /// this method is used to see the view of login
        /// </summary>
        /// <returns>Login form view</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            //-added by roshni
            if (_basicSetupRepository.IsFirstTimeUser())
                return RedirectToAction(nameof(HomeController.Setup), "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// this method will be called with credentials to validate user
        /// </summary>
        /// <returns>succesful login-checking for user and redirect to testdashboard page</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login loginModel, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, isPersistent: false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ViewBag.Error = _stringConstant.InvalidLoginError;
                    return View(loginModel);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, _stringConstant.InvalidModelError);
                return View(loginModel);
            }
        }

        /// <summary>
        /// this method is used to redirect to any local url link
        /// </summary>
        /// <param name="returnUrl">string type of url</param>
        /// <returns>redirect to a specific url</returns>
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
        #endregion

        #region ForgotPassword API
        /// <summary>
        /// this method is used  to see the view of forgot password form
        /// </summary>
        /// <returns>forgot password form view</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// this method will be called to validate the input email and send a link to the perticular email id to reset the password
        /// </summary>
        /// <param name="forgotPasswordModel">object of forgotpassword model</param>
        /// <returns>redirect to forgot password confirmation view</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
                if (user == null)
                {
                    ViewBag.Error = _stringConstant.InvalidEmailError;
                    return View("ForgotPassword");
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                var result = await _emailService.SendMailAsync(to: user.Email, from: _emailSettings.UserName,
                     body: $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>",
                    subject: "Reset Password");
                if (!result)
                {
                    ViewBag.EmailError = _stringConstant.FailedToSendEmailError;
                    return View(forgotPasswordModel);
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(forgotPasswordModel);
        }

        /// <summary>
        /// this method is used to display confirmation message after sending email to reset the password
        /// </summary>
        /// <returns>message on successful mail send</returns>
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region ResetPassword API
        /// <summary>
        /// this method is called when link is clicked through mail
        /// </summary>
        /// <param name="code">token generated during email send</param>
        /// <returns>reset password form view</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code)
        {

            return code == null ? View("Error") : View();
        }

        /// <summary>
        /// this method is used to take the new password and update with the existing password 
        /// </summary>
        /// <param name="resetPasswordModel">object of resetpassword model</param>
        /// <returns>redirect to reset password confirmation page</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
                if (currentUser == null)
                {
                    ViewBag.Error = _stringConstant.InvalidEmailError;
                    return View(resetPasswordModel);
                }

                var result = await _userManager.ResetPasswordAsync(currentUser, resetPasswordModel.Code, resetPasswordModel.ConfirmPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
                }
                else
                {
                    ViewBag.LinkError = _stringConstant.InvalidTokenError;
                    return View(resetPasswordModel);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, _stringConstant.InvalidModelError);
                return View(resetPasswordModel);
            }
        }

        /// <summary>
        /// this method is used to show the reset password confirmation message 
        /// </summary>
        /// <returns>reset password confirmation view</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion
        #endregion
    }
}