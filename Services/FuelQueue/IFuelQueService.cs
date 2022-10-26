/*
* IFuelQueueService: interface - Interface for fuel queue operations on database are managed
*/

using equeue_server.Models;

namespace equeue_server.Services
{
    public interface IFuelQueService
    {
        List<FuelQue> Get();
        FuelQue Get(string id);
        FuelQue Create(FuelQue fuelQueue);
        void Update(string id, FuelQue fuelQueue);
        bool AddUsersToQueue(QueueCustomer queueCustomer, string fuelStation);
        bool RemoveUsersFromQueue(string fuelStation, string customer, string detailedStatus);
        void Delete(string id);
    }
}
