using System;
using System.ComponentModel;

namespace Dyspozytornia.Models
{
    public class NewMapPointer
    {
        [DisplayName("Point ID")] public int PointId { get; set; }
        [DisplayName("Nazwa")] public string PointName { get; set; }
        [DisplayName("NIP")] public string Nip { get; set; }
        [DisplayName("Miasto")] public string PointCity { get; set; }
        [DisplayName("Adres")] public string PointAddress { get; set; }
        [DisplayName("Numer domu")] public string PointAddressBlockNumber { get; set; }
        [DisplayName("Longitude")] public double PointLongitude { get; set; }
        [DisplayName("Latitude")] public double PointLatitude { get; set; }
        [DisplayName("Typ")] public string PointType { get; set; }
        
        public string MyToString()
        {
            return "ID: " + PointId + " Name: " + PointName + " Lon: " + PointLongitude + " Lat: " + PointLatitude;
        }

        public bool Equals(NewMapPointer pointer){
            return PointLongitude == pointer.PointLongitude && PointLatitude == pointer.PointLatitude;
        }

        public bool Exists()
        {
            return PointId != 0;
        }
    }
}