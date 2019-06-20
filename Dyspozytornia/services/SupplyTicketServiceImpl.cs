using System;
using System.Collections;
using Dyspozytornia.dao;
using Dyspozytornia.Models;

namespace Dyspozytornia.services
{
    public class SupplyTicketServiceImpl: SupplyTicketService
    {
        private SupplyTicketDAO supplyTicketDAO;

        public void setSupplyTicketDAO(SupplyTicketDAO supplyTicketDAO) { this.supplyTicketDAO = supplyTicketDAO; }

        public ArrayList/*<SupplyTicket>*/ showTickets() {
            return supplyTicketDAO.createTicketTable();
        }

        public void createTicketNaive(SupplyTicket ticket) {
            this.supplyTicketDAO.createTicketNaive(ticket);
        }

        public void createTicketEntry(SupplyTicket ticket) {
            this.supplyTicketDAO.createTicketEntry(ticket);
        }

        public String getShopsName(int shopId) { return supplyTicketDAO.getShopsName(shopId); }

        public float getShopsLon(int shopId) {
            return supplyTicketDAO.getShopsLon(shopId);
        }

        public float getShopsLat(int shopId) {
            return supplyTicketDAO.getShopsLat(shopId);
        }


        public float getStoreLat(int storeId){
            return supplyTicketDAO.getStoreLat(storeId);

        }

        public float getStoreLon(int storeId){
            return supplyTicketDAO.getStoreLon(storeId);
        }

        public int[] getDriversByStoreId(int storeId) {
            return supplyTicketDAO.getDriversByStoreId(storeId);
        }

        public ArrayList/*<SupplyTicket>*/ getTicketsByDrivers(int[] drivers) {
            return supplyTicketDAO.getTicketsByDrivers(drivers);
        }

        public void createTicketNew(SupplyTicket ticket) {
            this.supplyTicketDAO.createTicketNew(ticket);
        }
    }
}