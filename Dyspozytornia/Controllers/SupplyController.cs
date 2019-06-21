using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyspozytornia.Models;
using Dyspozytornia.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dyspozytornia.Controllers
{
    public class SupplyController : Controller
    {
        private readonly ISupplyTicketService _ticketService;
        private readonly MapPointerService _pointerService;
        private const int MaxTicketsPerDriver = 3;

        public SupplyController()
        {
            _ticketService = new SupplyTicketService();
            _pointerService = new MapPointerService();
        }
        
        [Route("/supply")]
        [Authorize]
        public IActionResult Supply()
        {
            var supplyTickets = new ArrayList(_ticketService.ShowTickets());
            var tableFill = new StringBuilder("Obecnie nie mamy zadnych zamowien");

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
                    if (ticket.IsCompleted) continue;
                    var shopName = _ticketService.GetShopsName(ticket.ShopId);
                    var shopLon = _ticketService.GetShopsLon(ticket.ShopId);
                    var shopLat = _ticketService.GetShopsLat(ticket.ShopId);
                    var storeLon = _ticketService.GetStoreLon(ticket.ShopId);
                    var storeLat = _ticketService.GetStoreLat(ticket.ShopId);
    
                    var htmlTag = "<tr><td>" + ticket.TicketId + "</td><td>" + shopName +
                                  "</td><td style='display: none'>" + shopLon + "</td><td style='display: none'>" + shopLat +
                                  "</td><td style='display: none'>" + storeLon + "</td><td style='display: none'>" + storeLat +
                                  "</td><td>" + ticket.StoreId + "</td><td>"+ ticket.DriverId +"</td><td>"+
                                  ticket.Duration + "</td><td>" + ticket.TicketStatus + "</td><td>"+
                                  ticket.DeliveryDate + "</td><td>" + ticket.Path + "</td></tr>\n";
                    tableFill.Append(htmlTag);
                }
                tableFill.Append("</table>");
            }
            
            //model.addAttribute("deliveryTicketFill", tableFill.ToString());
            ViewBag.table = tableFill.ToString();
            return View();
        }

        [Route("/acceptDelivery")]
        [Authorize]
        public IActionResult AcceptDelivery()
        {
            var supplyTickets = _ticketService.GetPendingTickets();

            if (supplyTickets.Count < 3)
            {
                return RedirectToAction("Supply", "Supply");
            }
            
            //struktura wygląda następująco:        Map< STORE_ID, Map< DISTANCE, Map< TICKET_ID_LIST, SHOP_ID_LIST( == PATH) >>>
            var finalDistanceMap = new Dictionary<int, Dictionary<double, Dictionary<ArrayList, ArrayList>>>();

            var allWarehouses = _pointerService.ShowStoreTable();

            var nextFewTickets = new ArrayList();

            var ticketCounter = 0;
            foreach (SupplyTicket ticket in supplyTickets)
            {
                //log.info(ticket.toString());
                if (ticket.TicketStatus != "oczekujace")
                    continue;
                if (ticketCounter == MaxTicketsPerDriver)
                    break;
                nextFewTickets.Add(ticket);
                ticketCounter++;
            }

            var distanceMap = new Dictionary<int, Dictionary<double, Dictionary<ArrayList, ArrayList>>>();
            foreach (NewMapPointer warehouse in allWarehouses)
            {
                var distancePathMap = CalculateShortestDistanceAndPath(nextFewTickets, warehouse);

                distanceMap[warehouse.PointId] = distancePathMap;
            }

            double shortestDistance = 10000;
            foreach (var (warehouseId, distancePathMap) in distanceMap)
            {
                double distance = distancePathMap.ElementAt(0).Key;
                if (!(distance < shortestDistance)) continue;
                shortestDistance = distance;
                finalDistanceMap = new Dictionary<int, Dictionary<double, Dictionary<ArrayList, ArrayList>>>
                {
                    [warehouseId] = distancePathMap
                };
            }

            Console.WriteLine("Length 1: " + finalDistanceMap.Count);
            Console.WriteLine("Length 2: " + finalDistanceMap.Values.Count);

            var sicketIdAndPath =
                finalDistanceMap.ElementAt(0).Value.ElementAt(0).Value;

            ArrayList ticketIdList = sicketIdAndPath.ElementAt(0).Key;
            ArrayList pathList = sicketIdAndPath.ElementAt(0).Value;
            var pathId = -1;
            for (var n = 0; n < MaxTicketsPerDriver; n++)
            {
                var shopId = pathList[n];
                var ticketId = ticketIdList[n];
                var ticket = new SupplyTicket
                {
                    ShopId = (int) shopId,
                    TicketId = (int) ticketId,
                    ShopName = _ticketService.GetShopsName((int) shopId),
                    TicketStatus = "w realizacji"
                };
                //to powinno brać odległość
                var distance = finalDistanceMap.ElementAt(0).Value.ElementAt(0).Key;
                ticket.Duration = UtilsService.CalculateDuration(distance);
                ticket.Distance = distance;
                ticket.DriverId = 1;
                ticket.StoreId = finalDistanceMap.ElementAt(0).Key;
                ticket.DeliveryDate = "date";

                if (n == 0)
                    pathId = (int) ticketId;
                ticket.Path = pathId;
                _ticketService.CreateTicketNew(ticket);
            }


            return RedirectToAction("Supply", "Supply");
        }

        [Route("/supplyDeliveryRequest")]
        [Authorize]
        [HttpGet]
        public IActionResult SupplyDeliveryRequest()
        {
            var shopList = _pointerService.ShowShopTable();
            var shopOptions = new StringBuilder();
            var shopDay = new StringBuilder();
            var shopMonth = new StringBuilder();
            var shopYear = new StringBuilder();
            var shopHour = new StringBuilder();
            var shopMinute = new StringBuilder();

            foreach(NewMapPointer shop in shopList){
                var htmlTag = "<option>" + shop.PointName + "</option>";
                shopOptions.Append(htmlTag);
            }

            for (var i=1; i<=60; i++){
                if (i<= 12){
                    shopMonth.Append("<option>").Append(i).Append("</option>");
                }
                if(i<=31){
                    shopDay.Append("<option>").Append(i).Append("</option>");
                }
                if(i >= 8 && i< 16){
                    shopHour.Append("<option>").Append(i).Append("</option>");
                }
                if(i<=10) {
                    shopMinute.Append("<option>").Append("0").Append(i - 1).Append("</option>");
                }
                else
                    shopMinute.Append("<option>").Append(i - 1).Append("</option>");
                shopYear.Append("<option>").Append(2018 + i).Append("</option>");
            }


            ViewBag.supplyDeliveryRequestForm = new SupplyTicket();
            ViewBag.shopIdOptions = shopOptions.ToString();
            ViewBag.shopDay = shopDay.ToString();
            ViewBag.shopMonth = shopMonth.ToString();
            ViewBag.shopYear = shopYear.ToString();
            ViewBag.shopHour = shopHour.ToString();
            ViewBag.shopMinute = shopMinute.ToString();
            return View();
        }

        [Route("/supplyDeliveryRequest")]
        [HttpPost]
        [Authorize]
        public IActionResult CheckMapPointerRegister(string sklep, string dzien, string miesiac, string rok, string godzina, string minuta){
            var ticket = new SupplyTicket
            {
                ShopName = sklep,
                ShopDay = dzien,
                ShopHour = godzina,
                ShopMinute = minuta,
                ShopMonth = miesiac,
                ShopYear = rok
            };

            _ticketService.CreateTicketEntry(ticket);
            return RedirectToAction("supply");
        }

        private Dictionary<double, Dictionary<ArrayList, ArrayList>> CalculateShortestDistanceAndPath(ArrayList tickets,
                                                                              NewMapPointer warehouse){
        var distancePathMap = new Dictionary<double, Dictionary<ArrayList, ArrayList>>();

        var shortestDeliveryPath = new ArrayList();
        var ticketIds = new ArrayList();
        double shortestDistance = 1000;
        foreach(SupplyTicket ticket in tickets){
            ticketIds.Add(ticket.TicketId);

            var deliveryPath = new ArrayList();

            var shopName = _ticketService.GetShopsName(ticket.ShopId);
            var shop1 = _pointerService.GetPointerByName(shopName);

            var distance = UtilsService.CalculateDistanceInStraightLine(warehouse, shop1);
            deliveryPath.Add(shop1.PointId);

            SupplyTicket ticket2;
            SupplyTicket ticket3;
            
            if (tickets[0] == ticket){
                //pobieram id jednego z pozostałych ticketów, a potem wyznaczam na podstawie ID nazwę, żeby potem na
                // podstawie nazwy obliczyć odległość
                ticket2 = (SupplyTicket)tickets[1];
                ticket3 = (SupplyTicket)tickets[2];
            }
            else if (tickets[1] == ticket){
                ticket2 = (SupplyTicket)tickets[0];
                ticket3 = (SupplyTicket)tickets[2];
            }
            else{
                ticket2 = (SupplyTicket)tickets[0];
                ticket3 = (SupplyTicket)tickets[1];
            }
            var shopName2 = _ticketService.GetShopsName(ticket2.ShopId);
            var shopName3 = _ticketService.GetShopsName(ticket3.ShopId);

            var shop2 = _pointerService.GetPointerByName(shopName2);
            var shop3 = _pointerService.GetPointerByName(shopName3);

            var distance2 = UtilsService.CalculateDistanceInStraightLine(shop1, shop2);
            var distance3 = UtilsService.CalculateDistanceInStraightLine(shop1, shop3);

            var closer = Math.Min(distance2, distance3);

            distance += closer;

            distance += UtilsService.CalculateDistanceInStraightLine(shop2, shop3);
            NewMapPointer lastShop;
            if(distance2 == closer) {
                lastShop = shop3;
                deliveryPath.Add(shop2.PointId);
                deliveryPath.Add(shop3.PointId);
            }
            else {
                lastShop = shop2;
                deliveryPath.Add(shop3.PointId);
                deliveryPath.Add(shop2.PointId);
            }

            distance += UtilsService.CalculateDistanceInStraightLine(lastShop, warehouse);

            if (!(distance < shortestDistance)) continue;
            shortestDistance = distance;
            shortestDeliveryPath = deliveryPath;
        }
        var ticketsAndPath = new Dictionary<ArrayList, ArrayList>();
        ticketsAndPath[ticketIds] = shortestDeliveryPath;

        distancePathMap[shortestDistance] = ticketsAndPath;

        return distancePathMap;

    }
        private ArrayList CalculateWarehousesByTime(IEnumerable warehouses,
                                                               string date,
                                                               string hour,
                                                               NewMapPointer whereToDeliver){
            var calculatedWarehouses = new ArrayList();
            foreach(NewMapPointer store in warehouses){
                //function picks first suitable
                var distance = UtilsService.CalculateDistanceInStraightLine(whereToDeliver, store);
                var availableDrivers = CheckAvailableDrivers(store.PointId, distance, date, hour);
                if (availableDrivers == 0) continue;
                var calculatedStore = new Warehouse
                {
                    AvailableDrivers = availableDrivers,
                    StoreId = store.PointId,
                    Distance = distance,
                    DriverId = availableDrivers
                };

                calculatedWarehouses.Add(calculatedStore);
            }
            return calculatedWarehouses;
        }

        private int CheckAvailableDrivers(int storeId, double distance, String deliveryDate, String deliveryHour){
            var drivers = _ticketService.GetDriversByStoreId(storeId);
            var deliveryDuration = UtilsService.CalculateDuration(distance);
            var driversTickets = _ticketService.GetTicketsByDrivers(drivers);
            return (from driver in drivers let ifAvailableForDelivery = CheckAvailability(driver, driversTickets, deliveryDate, deliveryHour, deliveryDuration) where ifAvailableForDelivery select driver).FirstOrDefault();
        }

        private static bool CheckAvailability(int driverId,
                                                 IEnumerable driversTickets,
                                                 string deliveryDate,
                                                 string deliveryHour,
                                                 double deliveryDuration) {
            foreach(SupplyTicket ticket in driversTickets){
                if (ticket.DriverId != driverId) continue;
                var ticketFullDate = ticket.DeliveryDate.Split(" ")[0];
                var ticketFullTime = ticket.DeliveryDate.Split(" ")[1];
                if (!ticketFullDate.Equals(deliveryDate)) continue;
                var deliveryFullTime = deliveryHour.Split((":"));
                var ticketTime = ticketFullTime.Split((":"));
                var deliveryHourInt = int.Parse(deliveryFullTime[0]);
                var deliveryMinuteInt = int.Parse(deliveryFullTime[1]);
                var ticketHourInt = int.Parse(ticketTime[0]);
                var ticketMinuteInt = int.Parse(ticketTime[1]);

                //in minutes
                var difference = 60 * (Math.Abs(deliveryHourInt - ticketHourInt)) +
                                 (Math.Abs(deliveryMinuteInt - ticketMinuteInt));
                if((difference - deliveryDuration) - ticket.Duration < 0){
                    return false;
                }
            }
            return true;
        }
    }
}