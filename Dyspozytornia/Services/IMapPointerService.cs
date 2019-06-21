using System;
using System.Collections;
using Dyspozytornia.Models;

namespace Dyspozytornia.Data
{
    public interface IMapPointerService
    {
        ArrayList showStoreTable();
        ArrayList showShopTable();
        void createMapPointer(NewMapPointer mapPointer, String typeOfPoint);

        NewMapPointer getPointerByName(String shopName);
    }
}