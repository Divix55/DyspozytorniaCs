using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dyspozytornia.Controllers
{
    public class MapPointerController : Controller
    {
        [Route("/stores")]
        [Authorize]
        public IActionResult Stores()
        {
            return View();
        }

        [Route("/shops")]
        [Authorize]
        public IActionResult Shops()
        {
            return View();
        }

        [Route("/mapPointerRegister")]
        [Authorize]
        public string MapPointerRegisterGet()
        {
            return "test";
        }

        [Route("/mapPointerRegister")]
        [Authorize]
        public string MapPointerRegisterPost()
        {
            return "test";
        }
    }
}