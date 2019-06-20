using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections;
using System.Text;
using Dyspozytornia.Models;
using Dyspozytornia.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dyspozytornia.Controllers
{
    public class SupplyController : Controller
    {
        private readonly ISupplyTicketService ticketService;

        public SupplyController()
        {
            ticketService = new SupplyTicketService();
        }
        
        [Route("/supply")]
        [Authorize]
        public IActionResult Supply()
        {
            ArrayList supplyTickets = new ArrayList(ticketService.showTickets());
            StringBuilder tableFill = new StringBuilder("Obecnie nie mamy zadnych zamowien");

            if(supplyTickets.Count>0) {
                tableFill = new StringBuilder("<table id='tabela_dostawy'>" +
                        "<tr>" +
                        "<th style=\"width: 10%\">Numer zamowienia</th>" +
                        "<th>Nazwa sklepu" + "</th>" +
                        "<th style='display: none'>Lon</th>" +
                        "<th style='display: none'>Lat</th>" +
                        "<th>Store Id</th>" +
                        "<th>Driver Id</th>" +
                        "<th>Czas trwania</th>" +
                        "<th>Status</th>" +
                        "<th>Oczekiwana data dostawy</th>" +
                        "<th>Numer trasy</th>" +
                        "</tr>\n");
                foreach (SupplyTicket ticket in supplyTickets) {
                    if (!ticket.isComplet()) {
                        String shopName = ticketService.getShopsName(ticket.getShopId());
                        float shopLon = ticketService.getShopsLon(ticket.getShopId());
                        float shopLat = ticketService.getShopsLat(ticket.getShopId());
                        float storeLon = ticketService.getStoreLon(ticket.getStoreId());
                        float storeLat = ticketService.getStoreLat(ticket.getStoreId());
    
                        String htmlTag = "<tr><td>" + ticket.getTicketId() + "</td><td>" + shopName +
                                "</td><td style='display: none'>" + shopLon + "</td><td style='display: none'>" + shopLat +
                                "</td><td style='display: none'>" + storeLon + "</td><td style='display: none'>" + storeLat +
                                "</td><td>" + ticket.getStoreId() + "</td><td>"+ ticket.getDriverId() +"</td><td>"+
                                ticket.getDuration() + "</td><td>" + ticket.getTicketStatus() + "</td><td>"+
                                ticket.getDeliveryDate() + "</td><td>" + ticket.getPath() + "</td></tr>\n";
                        tableFill.Append(htmlTag);
                    }
                }
                tableFill.Append("</table>");
            }
    
            model.addAttribute("deliveryTicketFill", tableFill.ToString());
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
