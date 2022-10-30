using equeue_server.Models;

/*
* IFuleStationService: interface - Interface for manage fuel station operations on database
*/
namespace equeue_server.Services
{
    public interface IFuelStationService
    {
        List<FuelStation> Get();
        FuelStation Get(string id);
        List<FuelStation> GetOwnerStations(string id);
        FuelStation Create(FuelStation fuelStation);
        void Update(string id, FuelStation fuelStation);
        void Delete(string id);
    }
}