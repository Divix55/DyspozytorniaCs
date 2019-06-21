using System;

namespace Dyspozytornia.Models
{
    public class NewMapPointer
    {
        private int pointId;
        private String pointName;
        private String nip;
        private String pointCity;
        private String pointAddress;
        private String pointAddressBlockNumber;
        private double pointLongitude;
        private double pointLatitude;
        private String pointType;
        public void setPointLongitude(double pointLongitude) {
            this.pointLongitude = pointLongitude;
        }

        public void setPointLatitude(double pointLatitude) {
            this.pointLatitude = pointLatitude;
        }

        public String getNip() {
            return nip;
        }

        public void setNip(String nip) {
            this.nip = nip;
        }

        public String getPointName() {
            return pointName;
        }

        public void setPointName(String pointName) {
            this.pointName = pointName;
        }

        public double getPointLongitude() {
            return pointLongitude;
        }

        public double getPointLatitude() {
            return pointLatitude;
        }

        public void setPointId(int pointId) {
            this.pointId = pointId;
        }

        public int getPointId() { return this.pointId; }

        public String getPointCity() { return pointCity; }

        public void setPointCity(String pointCity) { this.pointCity = pointCity; }

        public String getPointAddress() { return pointAddress; }

        public void setPointAddress(String pointAddress) { this.pointAddress = pointAddress; }

        public String getPointAddressBlockNumber() { return pointAddressBlockNumber; }

        public void setPointAddressBlockNumber(String pointAddressBlockNumber) { this.pointAddressBlockNumber = pointAddressBlockNumber; }

        public String getPointType() {
            return pointType;
        }

        public void setPointType(String pointType) {
            this.pointType = pointType;
        }

        public NewMapPointer()
        {
            this.pointId = 0;
            this.pointName = "";
            this.nip = "";
            this.pointCity = "";
            this.pointAddress = "";
            this.pointAddressBlockNumber = "";
            this.pointLongitude = 0.0;
            this.pointLatitude = 0.0;
        }

        public NewMapPointer(int id, String name, String nip, String city, String address, String addresBlockNumber,
            double longitude, double latitude)
        {
            this.pointId = id;
            this.pointName = name;
            this.nip = nip;
            this.pointCity = city;
            this.pointAddress = address;
            this.pointAddressBlockNumber = addresBlockNumber;
            this.pointLongitude = longitude;
            this.pointLatitude = latitude;
        }

        public String toString()
        {
            return "ID: " + pointId + " Name: " + pointName + " Lon: " + pointLongitude + " Lat: " + pointLatitude;
        }

        public Boolean equals(NewMapPointer pointer){
            return getPointLongitude() == pointer.getPointLongitude() && getPointLatitude() == pointer.getPointLatitude();
        }

        public Boolean exists()
        {
            return this.pointId != 0;
        }
    }
}