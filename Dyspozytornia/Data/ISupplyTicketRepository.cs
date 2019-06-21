using System;
using System.Collections;
using System.Collections.Generic;
using Dyspozytornia.Models;

namespace Dyspozytornia.Data
{
    public interface ISupplyTicketRepository
    {
        ArrayList CreateTicketTable();
        void CreateTicketNaive(SupplyTicket ticket);
        void CreateTicketEntry(SupplyTicket ticket);
        String GetShopsName(int shopsId);
        float GetShopsLat(int shopsId);
        float GetShopsLon(int shopsId);
        float GetStoreLat(int storeId);
        float GetStoreLon(int storeId);
        int[] GetDriversByStoreId(int storeId);
        ArrayList GetTicketsByDrivers(IEnumerable<int> drivers);

        void CreateTicketNew(SupplyTicket ticket);
        ArrayList GetAllPendingTickets();
        string GetTicketDateById(int ticketId);
    }
}