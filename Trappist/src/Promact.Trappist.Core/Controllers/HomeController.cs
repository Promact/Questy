using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Promact.Trappist.Web.Controllers
{
    public class HomeController : Controller
    {      
        public HomeController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Setup(string returnUrl = null)
        {
            return View();
        }
    }
}
