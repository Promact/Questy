using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using Promact.Trappist.Repository.BasicSetup;

namespace Promact.Trappist.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Private variables
        #region Dependencies
        private readonly IBasicSetupRepository _basicSetup;
        #endregion
        #endregion

        #region Constructor
        public HomeController(IBasicSetupRepository basicSetup)
        {
            _basicSetup = basicSetup;
        }
        #endregion

        public IActionResult Index()
        {
            if (_basicSetup.IsFirstTimeUser())
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Setup), "Home");
            }
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Setup(string returnUrl = null)
        {
            if (_basicSetup.IsFirstTimeUser())
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                return View();
            }
        }

    }
}