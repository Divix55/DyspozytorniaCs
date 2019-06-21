using System;
using System.Collections;
using System.Threading.Tasks;
using Dyspozytornia.Models;

namespace Dyspozytornia.Data
{
    public interface IMapPointerService
    {
        ArrayList ShowStoreTable();
        ArrayList ShowShopTable();
        Task CreateMapPointer(NewMapPointer mapPointer, string typeOfPoint);
        NewMapPointer GetPointerByName(string shopName);
    }
}