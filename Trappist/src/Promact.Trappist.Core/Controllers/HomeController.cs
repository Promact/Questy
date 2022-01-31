﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.BasicSetup;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Utility.Constants;

namespace Promact.Trappist.Core.Controllers
{
    public class HomeController : Controller
    {
        #region Private variables
        #region Dependencies
        private readonly IBasicSetupRepository _basicSetup;
        private readonly ITestConductRepository _testConduct;

        #endregion
        #endregion

        #region Constructor
        public HomeController(IBasicSetupRepository basicSetup, ITestConductRepository testConduct, IStringConstants stringConstants)
        {
            _basicSetup = basicSetup;
            _testConduct = testConduct;
        }
        #endregion

        #region Public methods
        [Authorize]
        public IActionResult Index()
        {
            if (_basicSetup.IsFirstTimeUser())
                return RedirectToAction(nameof(HomeController.Setup), "Home");
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Setup()
        {
            if (!_basicSetup.IsFirstTimeUser())
                return RedirectToAction(nameof(HomeController.Index), "Home");
            return View();
        }

        /// <summary>
        /// This method is used to validate magic string.
        /// </summary>
        /// <param name="link">It contain magic string which uniquely identifies test</param>
        /// <returns>If link is valid than redirect to registration page else redirect to page not found action.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Conduct(string link)
        {
            var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            if (!string.IsNullOrWhiteSpace(link) && await _testConduct.IsTestLinkExistForTestConductionAsync(link, ipAddress))
            {
                ViewBag.Link = link;
                return View();
            }
            return RedirectToAction("PageNotFound");
        }

        [AllowAnonymous]
        public IActionResult PageNotFound()
        {
            return View();
        }
        #endregion
    }
}
