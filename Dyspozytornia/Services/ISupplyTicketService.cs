using System;
using System.Collections;
using Dyspozytornia.Models;

namespace Dyspozytornia.Services
{
    public interface ISupplyTicketService
    {
        ArrayList ShowTickets();
        void CreateTicketNaive(SupplyTicket ticket);
        void CreateTicketEntry(SupplyTicket ticket);
        String GetShopsName(int shopId);
        float GetShopsLon(int shopId);
        float GetShopsLat(int shopId);
        float GetStoreLat(int storeId);
        float GetStoreLon(int storeId);
        int[] GetDriversByStoreId(int storeId);
        ArrayList GetTicketsByDrivers(int[] drivers);
        void CreateTicketNew(SupplyTicket ticket);
    }
}