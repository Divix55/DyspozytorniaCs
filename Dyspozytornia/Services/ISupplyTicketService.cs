using System;
using System.Collections;
using Dyspozytornia.Models;

namespace Dyspozytornia.services
{
    public interface ISupplyTicketService
    {
        ArrayList showTickets();
        void createTicketNaive(SupplyTicket ticket);
        void createTicketEntry(SupplyTicket ticket);
        String getShopsName(int shopId);
        float getShopsLon(int shopId);
        float getShopsLat(int shopId);
        float getStoreLat(int storeId);
        float getStoreLon(int storeId);
        int[] getDriversByStoreId(int storeId);
        ArrayList getTicketsByDrivers(int[] drivers);

        void createTicketNew(SupplyTicket ticket);
    }
}