using System;
using System.Collections;
using Dyspozytornia.Models;

namespace Dyspozytornia.dao
{
    public interface SupplyTicketDAO
    {
        ArrayList createTicketTable();
        void createTicketNaive(SupplyTicket ticket);
        void createTicketEntry(SupplyTicket ticket);
        String getShopsName(int shopsId);
        float getShopsLat(int shopsId);
        float getShopsLon(int shopsId);
        float getStoreLat(int storeId);
        float getStoreLon(int storeId);
        int[] getDriversByStoreId(int storeId);
        ArrayList getTicketsByDrivers(int[] drivers);

        void createTicketNew(SupplyTicket ticket);
    }
}