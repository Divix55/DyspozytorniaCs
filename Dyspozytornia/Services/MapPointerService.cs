using System;
using System.Collections;
using Dyspozytornia.Data;
using Dyspozytornia.Models;

namespace Dyspozytornia.Services
{
    public class MapPointerService : IMapPointerService
    {
        private IMapPointerRepository imapPointerRepository;

        public MapPointerService()
        {
            imapPointerRepository = new MapPointerRepository();
        }
        
        public ArrayList showStoreTable() {return imapPointerRepository.createStoreTable(); }
        public ArrayList showShopTable() {return imapPointerRepository.createShopTable(); }

        public void createMapPointer(NewMapPointer mapPointer, String typeOfPoint) {
            this.imapPointerRepository.createMapPointer(mapPointer, typeOfPoint);
        }

        public NewMapPointer getPointerByName(String shopName) {
            return imapPointerRepository.getPointerByName(shopName);
        }
    }
}