using System;
using System.Diagnostics;
using Dyspozytornia.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dyspozytornia.Controllers
{
    public class LoginController : Controller
    {
        [Route("/login")]
        public IActionResult Login()
        {
            return View();
        }
        
        [Route("/register")]
        public IActionResult Rgister()
        {
            return View();
        }

        [Route("/logout")]
        public String Logout()
        {
            return "logout";
        }

        [Route("/account")]
        public IActionResult Account()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}