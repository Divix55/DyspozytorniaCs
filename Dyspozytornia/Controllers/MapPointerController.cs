using System;
using Microsoft.AspNetCore.Mvc;

namespace Dyspozytornia.Controllers
{
    public class MapPointerController : Controller
    {
        [Route("/stores")]
        public IActionResult Stores()
        {
            return View();
        }
        
        [Route("/shops")]
        public IActionResult Shops()
        {
            return View();
        }
        
        [Route("/mapPointerRegister")]
        public String MapPointerRegisterGet()
        {
            return "test";
        }
        
        [Route("/mapPointerRegister")]
        public String MapPointerRegisterPost()
        {
            return "test";
        }
    }
}