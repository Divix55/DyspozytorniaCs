using System;
using System.Collections;
using Dyspozytornia.Data;
using Dyspozytornia.Models;

namespace Dyspozytornia.Services
{
    public class SupplyTicketService: ISupplyTicketService
    {
        private readonly ISupplyTicketRepository _supplyTicketRepository;

        public SupplyTicketService()
        {
            _supplyTicketRepository = new SupplyTicketRepository();
        }

        public ArrayList ShowTickets() {
            return _supplyTicketRepository.CreateTicketTable();
        }

        public void CreateTicketNaive(SupplyTicket ticket) {
            _supplyTicketRepository.CreateTicketNaive(ticket);
        }

        public void CreateTicketEntry(SupplyTicket ticket) {
            _supplyTicketRepository.CreateTicketEntry(ticket);
        }

        public String GetShopsName(int shopId) { return _supplyTicketRepository.GetShopsName(shopId); }

        public float GetShopsLon(int shopId) {
            return _supplyTicketRepository.GetShopsLon(shopId);
        }

        public float GetShopsLat(int shopId) {
            return _supplyTicketRepository.GetShopsLat(shopId);
        }


        public float GetStoreLat(int storeId){
            return _supplyTicketRepository.GetStoreLat(storeId);

        }

        public float GetStoreLon(int storeId){
            return _supplyTicketRepository.GetStoreLon(storeId);
        }

        public int[] GetDriversByStoreId(int storeId) {
            return _supplyTicketRepository.GetDriversByStoreId(storeId);
        }

        public ArrayList GetTicketsByDrivers(int[] drivers) {
            return _supplyTicketRepository.GetTicketsByDrivers(drivers);
        }

        public void CreateTicketNew(SupplyTicket ticket) {
            _supplyTicketRepository.CreateTicketNew(ticket);
        }
    }
}