using System;

namespace Dyspozytornia.Models
{
    
    public class SupplyTicket
    { 
        public int TicketId { get; set; }
        public int StoreId { get; set; }
        public int ShopId { get; set; }
        public int DriverId { get; set; }
        public string DeliveryDate { get; set; }
        public bool IsCompleted { get; set; }
        public string ShopName { get; set; }
        public string TicketStatus { get; set; }
        public float ShopLon { get; set; }
        public float ShopLat { get; set; }
        public string ShopDay { get; set; }
        public string ShopMonth { get; set; }
        public string ShopYear { get; set; }
        public string ShopHour { get; set; }
        public string ShopMinute { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }
        public int Path { get; set; }
        
 
        public string MyToString(){ 
            return "TicketId: " + TicketId + " |StoreId: " + StoreId + " |ShopId: " + ShopId + " |ShopName: " + ShopName + " |DriverId: " + DriverId + 
                   " |DeliveryDate: " + DeliveryDate + " |Lon: " + ShopLon + " |Lat: " + ShopLat + " |Dist: " + Distance + 
                   " |Duration: " + Duration + " |Status: " + TicketStatus;
        }
    }
}