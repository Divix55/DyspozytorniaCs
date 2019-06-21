using System;
using System.Collections;
using System.Threading.Tasks;
using Dyspozytornia.Models;

namespace Dyspozytornia.Data
{
    public interface IMapPointerRepository
    {
        ArrayList CreateStoreTable();
        ArrayList CreateShopTable();
        void CreateMapPointer(NewMapPointer mapPointer, string typeOfPoint);
        NewMapPointer GetPointerByName(string shopName);
    }
}