using System;

namespace Dyspozytornia.Models
{
    
    public class Warehouse {
        private double distance;
        private int storeId;
        private int availableDrivers;
        private int driverId;

        public double getDistance() {
            return distance;
        }

        public void setDistance(double distance) {
            this.distance = distance;
        }

        public int getStoreId() {
            return storeId;
        }

        public void setStoreId(int storeId) {
            this.storeId = storeId;
        }

        public Warehouse(){
            this.distance = 0.0;
            this.storeId = 0;
        }

        public String toString(){
            return "ID: " + this.getStoreId() + " Dist: " + this.getDistance() + " Available drivers: " + this.getAvailableDrivers();
        }

        public int getAvailableDrivers() {
            return availableDrivers;
        }

        public void setAvailableDrivers(int availableDrivers) {
            this.availableDrivers = availableDrivers;
        }

        public int getDriverId() {
            return driverId;
        }

        public void setDriverId(int driverId) {
            this.driverId = driverId;
        }
    }

}