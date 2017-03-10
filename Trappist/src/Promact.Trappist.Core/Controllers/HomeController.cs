﻿using Microsoft.AspNetCore.Mvc;

namespace Promact.Trappist.Web.Controllers
{
    public class HomeController : Controller
    {
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
