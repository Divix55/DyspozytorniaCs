using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dyspozytornia.Controllers
{
    public class SupplyController : Controller
    {
        [Route("/supply")]
        [Authorize]
        public IActionResult Supply()
        {
            return View();
        }

        [Route("/acceptDelivery")]
        [Authorize]
        public IActionResult AcceptDelivery()
        {
            return View();
        }

        [Route("/supplyDeliveryRequest")]
        [Authorize]
        public string SupplyDeliveryRequestGet()
        {
            return "test";
        }

        [Route("/supplyDeliveryRequest")]
        [Authorize]
        public string SupplyDeliveryRequestPost()
        {
            return "test";
        }
    }
}