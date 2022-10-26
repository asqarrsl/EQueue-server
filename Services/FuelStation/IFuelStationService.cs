/*
* IFuleStationService: interface - Interface for managing fuel station operations on database
*/

using equeue_server.Models;

namespace equeue_server.Services
{
    public interface IFuelStationService
    {
        List<FuelStation> Get();
        FuelStation Get(string id);
        FuelStation Create(FuelStation fuelStation);
        void Update(string id, FuelStation fuelStation);
        void Delete(string id);
    }
}
