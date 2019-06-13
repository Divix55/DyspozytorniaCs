using System;
using Microsoft.AspNetCore.Mvc;

namespace Dyspozytornia.Controllers
{
    public class SupplyController : Controller
    {
        [Route("/supply")]
        public IActionResult Supply()
        {
            return View();
        }
        
        [Route("/acceptDelivery")]
        public IActionResult AcceptDelivery()
        {
            return View();
        }
        
        [Route("/supplyDeliveryRequest")]
        public String SupplyDeliveryRequestGet()
        {
            return "test";
        }
        
        [Route("/supplyDeliveryRequest")]
        public String SupplyDeliveryRequestPost()
        {
            return "test";
        }
    }
}