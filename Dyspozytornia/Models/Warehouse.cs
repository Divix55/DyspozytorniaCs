namespace Dyspozytornia.Models
{
    
    public class Warehouse {
        public double Distance { get; set; }
        public int StoreId { get; set; }
        public int AvailableDrivers { get; set; }
        public int DriverId { get; set; }
        

        public Warehouse(){
            Distance = 0.0;
            StoreId = 0;
        }

        public string toString()
        {
            return "ID: " + StoreId + " Dist: " + Distance + " Available drivers: " +
                   AvailableDrivers;
        }
    }

}