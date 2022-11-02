using System;
using equeue_server.Models.Database;
using equeue_server.Services;
using equeue_server.Models;
using MongoDB.Driver;

/*
* FuelQueService: class Implements IFuelQueService: interface - Manages fuel queue operations on database
*/
namespace equeue_server.Services
{
    public class FuelQueService : IFuelQueService
    {
        // variable to hold mongodb collection
        private readonly IMongoCollection<FuelQue> _fuelQueue;

        // constructor - retrives collections and assign collection to _fuelQueue
        public FuelQueService(IStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _fuelQueue = database.GetCollection<FuelQue>(settings.FuelQueCollectionName);
        }

        /*
         * Function - Adding users to the fuel queue
         * Params - queueCustomer(QueueCustomer) - QueueCustomer object to add customers array
         *        - fuelStation(string) - id of fuel station
         * Returns - boolean (fuel queue updated status)
         */
        public bool AddUsersToQueue(QueueCustomer queueCustomer, string fuelStation)
        {
            // Auto generating entering time
            queueCustomer.enteredTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            queueCustomer.exitedTime = "";

            // Filtering fuelQueue using fuel station id
            var fuelStationFilter = Builders<FuelQue>
             .Filter.Eq(e => e.FuelStationId, fuelStation);

            // Pushing queue customer to the customers array
            var fuelQueueUpdate = Builders<FuelQue>.Update
                    .Push(e => e.Customers, queueCustomer);

            // Incrementing total number of vehicles in the queue
            var fuelQueueIncrementUpdate = Builders<FuelQue>.Update
                    .Inc(e => e.NumberOfVehicles, 1);

            // Commiting updates to database
            var fuelQueueUpdateResult = _fuelQueue.UpdateOne(fuelStationFilter, fuelQueueUpdate);
            var incrementUpdateResult = _fuelQueue.UpdateOne(fuelStationFilter, fuelQueueIncrementUpdate);

            // Return update status
            if (fuelQueueUpdateResult == null || incrementUpdateResult == null) {
                return false;
            }

            return true;

        }

        /*
         * Function - Removing users from the fuel queue
         * Params - fuelStation(string) - id of fuel station
         *        - customer(string) - id of vehicle owner
         *        - detailsedStatus(string) - reason to leave fuel queue (exit before pump / exit after pump)
         * Returns - boolean (fuel queue updated status)
         */
        public bool RemoveUsersFromQueue(string fuelStation, string customer, string detailedStatus)
        {
            // Finding fuel queue using fuel station id
            var fuelQueue = _fuelQueue.Find(fuelQueue => fuelQueue.FuelStationId == fuelStation).FirstOrDefault();

            // Making customers list in the fuel queue
            List<QueueCustomer> queueCustomers = fuelQueue.Customers.ToList();

            // Updating status and exited time using vehicle owner id
            foreach (QueueCustomer queueCustomer in queueCustomers)
            {
                if (queueCustomer.UserId == customer && queueCustomer.Status)
                {
                    queueCustomer.Status = false;
                    queueCustomer.DetailedStatus = detailedStatus;
                    queueCustomer.exitedTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                }
            }

            // Filtering fuelQueue using fuel station id
            var fuelStationFilter = Builders<FuelQue>
             .Filter.Eq(e => e.FuelStationId, fuelStation);

            // Setting queue customers array
            var fuelQueueUpdate = Builders<FuelQue>.Update.Set("customers", queueCustomers);

            // Decrementing total number of vehicles in the queue
            var fuelQueueDecrementUpdate = Builders<FuelQue>.Update
                   .Inc(e => e.NumberOfVehicles, -1);

            // Commiting updates to database
            var fuelQueueUpdateResult = _fuelQueue.UpdateOne(Builders<FuelQue>
             .Filter.Eq(e => e.FuelStationId, fuelStation), fuelQueueUpdate);
            var decrementUpdateResult = _fuelQueue.UpdateOne(fuelStationFilter, fuelQueueDecrementUpdate);

            // Return update status
            if (fuelQueueUpdateResult == null || decrementUpdateResult == null)
            {
                return false;
            }

            return true;
        }

        /*
         * Function - Register fuel queue to the fuel station
         * Params - fuelQueue(FuelQue) - FuelQue object to register
         * Returns - registered fuel queue(FuelQue)
         */
        public FuelQue Create(FuelQue fuelQueue)
        {
            _fuelQueue.InsertOne(fuelQueue);
            return fuelQueue;
        }

        /*
        * Function - Deleting fuel queue
        * Params - id(string) - fuel queue id to remove
        * Returns - void
        */
        public void Delete(string id)
        {
            _fuelQueue.DeleteOne(fuelQueue => fuelQueue.Id == id);
        }

        /*
        * Function - Retrieving fuel queues
        * Params - no params
        * Returns - List<FuelQue> list of fuel queue objects
        */
        public List<FuelQue> Get()
        {
            return _fuelQueue.Find(fuelQueue => true).ToList();
        }

        /*
        * Function - Retrieving fuel queue
        * Params - id(string) - fuel queue id to retrive
        * Returns - FuelQue fuelQueue object associated with id
        */
        public FuelQue Get(string id)
        {
            return _fuelQueue.Find(fuelQueue => fuelQueue.Id == id).FirstOrDefault();
        }

        /*
        * Function - Retrieving fuel queue by fuel station id
        * Params - id(string) - fuel station id to retrive
        * Returns - FuelQue fuelQueue object associated with id
        */
        public FuelQue GetFuelQueByFuelStationId(string id)
        {
            return _fuelQueue.Find(fuelQueue => fuelQueue.FuelStationId == id).FirstOrDefault();
        }

        /*
        * Function - Updating fuel queue
        * Params - id(string) - fuel queue id to retrive
        * Returns - FuelQue fuel queue object associated with id
        */
        public void Update(string id, FuelQue fuelQueue)
        {
            _fuelQueue.ReplaceOne(fuelQueue => fuelQueue.Id == id, fuelQueue);
        }
    }
}