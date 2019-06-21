using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Dyspozytornia.Models;
using Dyspozytornia.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dyspozytornia.Controllers
{
    public class MapPointerController : Controller
    {
        private readonly MapPointerService _pointerService;
        public MapPointerController() 
        {
            _pointerService = new MapPointerService();
        }
        
        [Route("/stores")]
        [Authorize]
        public IActionResult Stores()
        {
            ArrayList mapPointer = _pointerService.ShowStoreTable();
            ViewBag.table = TableFillerFunction(mapPointer);
            return View();
        }

        [Route("/shops")]
        [Authorize]
        public IActionResult Shops()
        {
            ArrayList mapPointer = _pointerService.ShowShopTable();
            ViewBag.table = TableFillerFunction(mapPointer);
            return View();
        }

        [Route("/mapPointerRegister")]
        [Authorize]
        [HttpGet]
        public IActionResult MapPointerRegister()
        {
            return View();
        }

        [Route("/mapPointerRegister")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult>  MapPointerRegister(NewMapPointer newMapPointer)
        {
            if (ModelState.IsValid)
            {
                await _pointerService.CreateMapPointer(newMapPointer, "Shops");
                return RedirectToAction("Shops", "MapPointer");
            }
            return RedirectToAction("MapPointerRegister", "MapPointer");
        }
        
        private String TableFillerFunction(ArrayList mapPointer) {
            StringBuilder tableFill = new StringBuilder();
            foreach(NewMapPointer point in mapPointer) {
                String htmlTag = "<tr class=\"wiersz\"><td class=\"name\">" + point.pointName + "</td>" +
                                 "<td style=\"display:none\" class=\"lat\">" + point.pointLatitude +
                                 "</td><td style=\"display:none\" class=\"lon\">" + point.pointLongitude + "</td>" +
                                 "</td><td class=\"nip\">" + point.nip + "</td>" +
                                 "</td><td class=\"city\">" + point.pointCity + "</td>" +
                                 "</td><td class=\"address\">" + point.pointAddress + "</td>" +
                                 "</td><td class=\"homenumber\">" + point.pointAddressBlockNumber + "</td>" +
                                 "</tr>";
                tableFill.Append(htmlTag);
            }

            return tableFill.ToString();
        }
    }
}