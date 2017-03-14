using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Promact.Trappist.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
