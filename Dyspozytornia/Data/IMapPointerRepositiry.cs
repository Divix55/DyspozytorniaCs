using System;
using System.Collections;
using Dyspozytornia.Models;

namespace Dyspozytornia.Data
{
    public interface IMapPointerRepository
    {
        ArrayList createStoreTable();
        ArrayList createShopTable();
        void createMapPointer(NewMapPointer mapPointer, String typeOfPoint);
        NewMapPointer getPointerByName(String shopName);
    }
}