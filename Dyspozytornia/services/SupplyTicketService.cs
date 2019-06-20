using System;
using System.Collections;
using Dyspozytornia.Data;
using Dyspozytornia.Models;
// Dd
namespace Dyspozytornia.services
{
    public class SupplyTicketService: ISupplyTicketService
    {
        private ISupplyTicketRepository _supplyTicketRepository;

        public SupplyTicketService()
        {
            _supplyTicketRepository = new SupplyTicketRepository();
        }

        public ArrayList/*<SupplyTicket>*/ showTickets() {
            return _supplyTicketRepository.createTicketTable();
        }

        public void createTicketNaive(SupplyTicket ticket) {
            _supplyTicketRepository.createTicketNaive(ticket);
        }

        public void createTicketEntry(SupplyTicket ticket) {
            _supplyTicketRepository.createTicketEntry(ticket);
        }

        public String getShopsName(int shopId) { return _supplyTicketRepository.getShopsName(shopId); }

        public float getShopsLon(int shopId) {
            return _supplyTicketRepository.getShopsLon(shopId);
        }

        public float getShopsLat(int shopId) {
            return _supplyTicketRepository.getShopsLat(shopId);
        }


        public float getStoreLat(int storeId){
            return _supplyTicketRepository.getStoreLat(storeId);

        }

        public float getStoreLon(int storeId){
            return _supplyTicketRepository.getStoreLon(storeId);
        }

        public int[] getDriversByStoreId(int storeId) {
            return _supplyTicketRepository.getDriversByStoreId(storeId);
        }

        public ArrayList/*<SupplyTicket>*/ getTicketsByDrivers(int[] drivers) {
            return _supplyTicketRepository.getTicketsByDrivers(drivers);
        }

        public void createTicketNew(SupplyTicket ticket) {
            this._supplyTicketRepository.createTicketNew(ticket);
        }
    }
}