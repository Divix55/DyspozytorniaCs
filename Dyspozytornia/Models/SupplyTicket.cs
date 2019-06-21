using System;

namespace Dyspozytornia.Models
{
    
    public class SupplyTicket
    { 
        public int ticketId { get; set; }
        public int storeId { get; set; }
        public int shopId { get; set; }
        public int driverId { get; set; }
        public String deliveryDate { get; set; }
        public bool isCompleted { get; set; }
        public String shopName { get; set; }
        public String ticketStatus { get; set; }
        public float shopLon { get; set; }
        public float shopLat { get; set; }
        public String shopDay { get; set; }
        public String shopMonth { get; set; }
        public String shopYear { get; set; }
        public String shopHour { get; set; }
        public String shopMinute { get; set; }
        public double distance { get; set; }
        public double duration { get; set; }
        public int path { get; set; }
        
 
        public String MyToString(){ 
            return "TicketId: " + ticketId + " |StoreId: " + storeId + " |ShopId: " + shopId + " |ShopName: " + shopName + " |DriverId: " + driverId + 
                   " |DeliveryDate: " + deliveryDate + " |Lon: " + shopLon + " |Lat: " + shopLat + " |Dist: " + distance + 
                   " |Duration: " + duration + " |Status: " + ticketStatus;
        }
    }
}