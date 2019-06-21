using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private readonly int MAX_TICKETS_PER_DRIVER = 3;

        public SupplyController()
        {
            _ticketService = new SupplyTicketService();
            _pointerService = new MapPointerService();
        }
        
        [Route("/supply")]
        [Authorize]
        public IActionResult Supply()
        {
            ArrayList supplyTickets = new ArrayList(_ticketService.ShowTickets());
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
                    if (!ticket.isCompleted) {
                        String shopName = _ticketService.GetShopsName(ticket.shopId);
                        float shopLon = _ticketService.GetShopsLon(ticket.shopId);
                        float shopLat = _ticketService.GetShopsLat(ticket.shopId);
                        float storeLon = _ticketService.GetStoreLon(ticket.shopId);
                        float storeLat = _ticketService.GetStoreLat(ticket.shopId);
    
                        String htmlTag = "<tr><td>" + ticket.ticketId + "</td><td>" + shopName +
                                "</td><td style='display: none'>" + shopLon + "</td><td style='display: none'>" + shopLat +
                                "</td><td style='display: none'>" + storeLon + "</td><td style='display: none'>" + storeLat +
                                "</td><td>" + ticket.storeId + "</td><td>"+ ticket.driverId +"</td><td>"+
                                ticket.duration + "</td><td>" + ticket.ticketStatus + "</td><td>"+
                                ticket.deliveryDate + "</td><td>" + ticket.path + "</td></tr>\n";
                        tableFill.Append(htmlTag);
                    }
                }
                tableFill.Append("</table>");
            }
            
            //model.addAttribute("deliveryTicketFill", tableFill.ToString());
            ViewBag.table = tableFill.ToString();
            return View();
        }

        [Route("/acceptDelivery")]
        [Authorize]
        public IActionResult AcceptDelivery(ArrayList supplyTickets){
            
            //struktura wygląda następująco:        Map< STORE_ID, Map< DISTANCE, Map< TICKET_ID_LIST, SHOP_ID_LIST( == PATH) >>>
            Dictionary<int, Dictionary<Double, Dictionary<ArrayList, ArrayList>>> finalDistanceMap = new Dictionary<int, Dictionary<double, Dictionary<ArrayList, ArrayList>>>();

            ArrayList allWarehouses = _pointerService.ShowStoreTable();

            ArrayList nextFewTickets = new ArrayList();

            int ticketCounter = 0;
            foreach(SupplyTicket ticket in supplyTickets) {
                //log.info(ticket.toString());
                if (ticket.ticketStatus != "oczekujace")
                    continue;
                if (ticketCounter == MAX_TICKETS_PER_DRIVER)
                    break;
                nextFewTickets.Add(ticket);
                ticketCounter++;
            }

            Dictionary<int, Dictionary<Double, Dictionary<ArrayList, ArrayList>>> distanceMap = new Dictionary<int, Dictionary<double, Dictionary<ArrayList, ArrayList>>>();
            foreach(NewMapPointer warehouse in allWarehouses){
                Dictionary<Double, Dictionary<ArrayList, ArrayList>> distance_path_map;
                distance_path_map = CalculateShortestDistanceAndPath(nextFewTickets, warehouse);

                distanceMap[warehouse.pointId] = distance_path_map;
            }

            double shortestDistance = 10000;
            foreach(KeyValuePair<int, Dictionary<Double, Dictionary<ArrayList, ArrayList>>> shortestPathPerWarehouse in distanceMap){
                int warehouseId = shortestPathPerWarehouse.Key;
                Dictionary<Double, Dictionary<ArrayList, ArrayList>> distance_path_map = shortestPathPerWarehouse.Value;
                double distance = distance_path_map.GetEnumerator().MoveNext().GetHashCode();
                if(distance < shortestDistance){
                    shortestDistance = distance;
                    finalDistanceMap = new Dictionary<int, Dictionary<double, Dictionary<ArrayList, ArrayList>>>();
                    finalDistanceMap[warehouseId] = shortestPathPerWarehouse.Value;
                }
            }

            finalDistanceMap.GetEnumerator().MoveNext();
            finalDistanceMap.GetEnumerator().Current.Value.GetEnumerator().MoveNext();
            Dictionary<ArrayList, ArrayList> sicketId_and_path =
                finalDistanceMap.GetEnumerator().Current.Value.GetEnumerator().Current.Value;

            ArrayList ticketIdList = sicketId_and_path.GetEnumerator().Current.Key;
            ArrayList pathList = sicketId_and_path.GetEnumerator().Current.Value;
            int pathId = -1;
            for(int n = 0; n < MAX_TICKETS_PER_DRIVER; n++){
                int shopId = pathList.IndexOf(n);
                int ticketId = ticketIdList.IndexOf(n);
                SupplyTicket ticket = new SupplyTicket();
                ticket.shopId = shopId;
                ticket.ticketId = ticketId;
                ticket.shopName = _ticketService.GetShopsName(shopId);
                ticket.ticketStatus = "w realizacji";
                //to powinno brać odległość
                double distance = finalDistanceMap.GetEnumerator().Current.Value.GetEnumerator().Current.Key;
                ticket.duration = UtilsService.CalculateDuration(distance);
                ticket.distance = distance;
                ticket.driverId = 1;
                ticket.storeId = finalDistanceMap.GetEnumerator().Current.Key;
                ticket.deliveryDate = "date";
                
                if(n==0)
                    pathId = ticketId;
                ticket.path = pathId;
                _ticketService.CreateTicketNew(ticket);
            }
            

            return View();
        }

        [Route("/supplyDeliveryRequest")]
        [Authorize]
        [HttpGet]
        public IActionResult SupplyDeliveryRequest()
        {
            ArrayList shopList = _pointerService.ShowShopTable();
            StringBuilder shopOptions = new StringBuilder();
            StringBuilder shopDay = new StringBuilder();
            StringBuilder shopMonth = new StringBuilder();
            StringBuilder shopYear = new StringBuilder();
            StringBuilder shopHour = new StringBuilder();
            StringBuilder shopMinute = new StringBuilder();

            foreach(NewMapPointer shop in shopList){
                String htmlTag = "<option>" + shop.pointName + "</option>";
                shopOptions.Append(htmlTag);
            }

            for (int i=1; i<=60; i++){
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
        public String CheckMapPointerRegister(SupplyTicket ticket){
            _ticketService.CreateTicketEntry(ticket);
            return "redirect:/supply";
        }

        private Dictionary<Double, Dictionary<ArrayList, ArrayList>> CalculateShortestDistanceAndPath(ArrayList tickets,
                                                                              NewMapPointer warehouse){
        Dictionary<Double, Dictionary<ArrayList, ArrayList>> distance_path_map = new Dictionary<double, Dictionary<ArrayList, ArrayList>>();

        ArrayList shortestDeliveryPath = new ArrayList();
        ArrayList ticketIds = new ArrayList();
        double shortestDistance = 1000;
        foreach(SupplyTicket ticket in tickets){
            ticketIds.Add(ticket.ticketId);

            ArrayList deliveryPath = new ArrayList();

            String shopName = _ticketService.GetShopsName(ticket.shopId);
            NewMapPointer shop1 = _pointerService.GetPointerByName(shopName);

            double distance = UtilsService.CalculateDistanceInStraightLine(warehouse, shop1);
            deliveryPath.Add(shop1.pointId);

            SupplyTicket ticket2 = new SupplyTicket();
            SupplyTicket ticket3 = new SupplyTicket();
            
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
            String shopName2 = _ticketService.GetShopsName(ticket2.shopId);
            String shopName3 = _ticketService.GetShopsName(ticket3.shopId);

            NewMapPointer shop2 = _pointerService.GetPointerByName(shopName2);
            NewMapPointer shop3 = _pointerService.GetPointerByName(shopName3);

            double distance2 = UtilsService.CalculateDistanceInStraightLine(shop1, shop2);
            double distance3 = UtilsService.CalculateDistanceInStraightLine(shop1, shop3);

            double closer = Math.Min(distance2, distance3);

            distance += closer;

            distance += UtilsService.CalculateDistanceInStraightLine(shop2, shop3);
            NewMapPointer lastShop = new NewMapPointer();
            if(distance2 == closer) {
                lastShop = shop3;
                deliveryPath.Add(shop2.pointId);
                deliveryPath.Add(shop3.pointId);
            }
            else {
                lastShop = shop2;
                deliveryPath.Add(shop3.pointId);
                deliveryPath.Add(shop2.pointId);
            }

            distance += UtilsService.CalculateDistanceInStraightLine(lastShop, warehouse);

            if(distance < shortestDistance){
                shortestDistance = distance;
                shortestDeliveryPath = deliveryPath;
            }
        }
        Dictionary<ArrayList, ArrayList> tickets_and_path = new Dictionary<ArrayList, ArrayList>();
        tickets_and_path[ticketIds] = shortestDeliveryPath;

        distance_path_map[shortestDistance] = tickets_and_path;

        return distance_path_map;

    }
        private ArrayList CalculateWarehousesByTime(ArrayList warehouses,
                                                               String date,
                                                               String hour,
                                                               NewMapPointer whereToDeliver){
            ArrayList calculatedWarehouses = new ArrayList();
            foreach(NewMapPointer store in warehouses){
                //function picks first suitable
                double distance = UtilsService.CalculateDistanceInStraightLine(whereToDeliver, store);
                int availableDrivers = CheckAvailableDrivers(store.pointId, distance, date, hour);
                if (availableDrivers != 0) {

                    Warehouse calculatedStore = new Warehouse();
                    calculatedStore.setAvailableDrivers(availableDrivers);
                    calculatedStore.setStoreId(store.pointId);
                    calculatedStore.setDistance(distance);
                    calculatedStore.setDriverId(availableDrivers);

                    calculatedWarehouses.Add(calculatedStore);
                }
            }
            return calculatedWarehouses;
        }

        private int CheckAvailableDrivers(int storeId, double distance, String deliveryDate, String deliveryHour){
            int[] drivers = _ticketService.GetDriversByStoreId(storeId);
            double deliveryDuration = UtilsService.CalculateDuration(distance);
            ArrayList driversTickets = _ticketService.GetTicketsByDrivers(drivers);
            foreach(int driver in drivers){
                bool ifAvailableForDelivery = CheckAvailability(driver, driversTickets, deliveryDate, deliveryHour, deliveryDuration);
                if(ifAvailableForDelivery){
                    return driver;
                }
            }

            return 0;
        }

        private bool CheckAvailability(int driverId,
                                                 ArrayList driversTickets,
                                                 String deliveryDate,
                                                 String deliveryHour,
                                                 double deliveryDuration) {
            foreach(SupplyTicket ticket in driversTickets){
                if (ticket.driverId == driverId){
                    String ticketFullDate = ticket.deliveryDate.Split(" ")[0];
                    String ticketFullTime = ticket.deliveryDate.Split(" ")[1];
                    if (ticketFullDate.Equals(deliveryDate)){
                        String[] deliveryFullTime = deliveryHour.Split((":"));
                        String[] ticketTime = ticketFullTime.Split((":"));
                        int deliveryHourInt = Int32.Parse(deliveryFullTime[0]);
                        int deliveryMinuteInt = Int32.Parse(deliveryFullTime[1]);
                        int ticketHourInt = Int32.Parse(ticketTime[0]);
                        int ticketMinuteInt = Int32.Parse(ticketTime[1]);

                        //in minutes
                        int difference = 60 * (Math.Abs(deliveryHourInt - ticketHourInt)) +
                                (Math.Abs(deliveryMinuteInt - ticketMinuteInt));
                        if((difference - deliveryDuration) - ticket.duration < 0){
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}