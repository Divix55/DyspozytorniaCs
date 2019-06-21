using System;
using System.ComponentModel;

namespace Dyspozytornia.Models
{
    public class NewMapPointer
    {
        [DisplayName("Point ID")] public int pointId { get; set; }
        [DisplayName("Nazwa")] public string pointName { get; set; }
        [DisplayName("NIP")] public string nip { get; set; }
        [DisplayName("Miasto")] public string pointCity { get; set; }
        [DisplayName("Adres")] public string pointAddress { get; set; }
        [DisplayName("Numer domu")] public string pointAddressBlockNumber { get; set; }
        [DisplayName("Longitude")] public double pointLongitude { get; set; }
        [DisplayName("Latitude")] public double pointLatitude { get; set; }
        [DisplayName("Typ")] public string pointType { get; set; }
        
        public String MyToString()
        {
            return "ID: " + pointId + " Name: " + pointName + " Lon: " + pointLongitude + " Lat: " + pointLatitude;
        }

        public bool Equals(NewMapPointer pointer){
            return pointLongitude == pointer.pointLongitude && pointLatitude == pointer.pointLatitude;
        }

        public bool Exists()
        {
            return pointId != 0;
        }
    }
}