using System;
using System.Collections;
using System.Text;
using Dyspozytornia.Models;
using Dyspozytornia.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Dyspozytornia.Controllers
{
    public class MapPointerController : Controller
    {
        private readonly MapPointerService pointerService;
        public MapPointerController() 
        {
            pointerService = new MapPointerService();
        }
        
        [Route("/stores")]
        [Authorize]
        public IActionResult Stores()
        {
            ArrayList mapPointer = pointerService.showStoreTable();
            ViewBag.table = tableFillerFunction(mapPointer);
            return View();
        }

        [Route("/shops")]
        [Authorize]
        public IActionResult Shops()
        {
            ArrayList mapPointer = pointerService.showShopTable();
            ViewBag.table = tableFillerFunction(mapPointer);
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
        
        private String tableFillerFunction(ArrayList mapPointer) {
            StringBuilder tableFill = new StringBuilder();
            foreach(NewMapPointer point in mapPointer) {
                String htmlTag = "<tr class=\"wiersz\"><td class=\"name\">" + point.getPointName() + "</td>" +
                                 "<td style=\"display:none\" class=\"lat\">" + point.getPointLatitude() +
                                 "</td><td style=\"display:none\" class=\"lon\">" + point.getPointLongitude() + "</td>" +
                                 "</td><td class=\"nip\">" + point.getNip() + "</td>" +
                                 "</td><td class=\"city\">" + point.getPointCity() + "</td>" +
                                 "</td><td class=\"address\">" + point.getPointAddress() + "</td>" +
                                 "</td><td class=\"homenumber\">" + point.getPointAddressBlockNumber() + "</td>" +
                                 "</tr>";
                tableFill.Append(htmlTag);
            }

            return tableFill.ToString();
            // model.addAttribute("mapPointerFill", tableFill.ToString());
        }
    }
}