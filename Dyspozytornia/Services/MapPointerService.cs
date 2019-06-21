using System.Collections;
using System.Threading.Tasks;
using Dyspozytornia.Data;
using Dyspozytornia.Models;

namespace Dyspozytornia.Services
{
    public class MapPointerService : IMapPointerService
    {
        private readonly IMapPointerRepository _iMapPointerRepository;

        public MapPointerService()
        {
            _iMapPointerRepository = new MapPointerRepository();
        }
        
        public ArrayList ShowStoreTable() {return _iMapPointerRepository.CreateStoreTable(); }
        public ArrayList ShowShopTable() {return _iMapPointerRepository.CreateShopTable(); }

        public async Task CreateMapPointer(NewMapPointer mapPointer, string typeOfPoint) { 
            await Task.Run(() => _iMapPointerRepository.CreateMapPointer(mapPointer, typeOfPoint));
        }

        public NewMapPointer GetPointerByName(string shopName) {
            return _iMapPointerRepository.GetPointerByName(shopName);
        }
    }
}