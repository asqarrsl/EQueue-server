using equeue_server.Models;
using System;

/*
* IFuelQueService: interface - Interface for manage fuel queue operations on database
*/
namespace equeue_server.Services
{
    public interface IFuelQueService
    {
        List<FuelQue> Get();
        FuelQue Get(string id);
        FuelQue Create(FuelQue FuelQue);
        FuelQue GetFuelQueByFuelStationId(string id);
        void Update(string id, FuelQue FuelQue);
        bool AddUsersToQueue(QueueCustomer queueCustomer, string fuelStation);
        bool RemoveUsersFromQueue(string fuelStation, string customer, string detailedStatus);
        void Delete(string id);
    }
}