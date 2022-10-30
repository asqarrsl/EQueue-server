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
        private readonly IMongoCollection<FuelQue> _FuelQue;

        // constructor - retrives collections and assign collection to _FuelQue
        public FuelQueService(IStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _FuelQue = database.GetCollection<FuelQue>(settings.FuelQueCollectionName);
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

            // Filtering FuelQue using fuel station id
            var fuelStationFilter = Builders<FuelQue>
             .Filter.Eq(e => e.FuelStationId, fuelStation);

            // Pushing queue customer to the customers array
            var FuelQueUpdate = Builders<FuelQue>.Update
                    .Push(e => e.Customers, queueCustomer);

            // Incrementing total number of vehicles in the queue
            var FuelQueIncrementUpdate = Builders<FuelQue>.Update
                    .Inc(e => e.NumberOfVehicles, 1);

            // Commiting updates to database
            var FuelQueUpdateResult = _FuelQue.UpdateOne(fuelStationFilter, FuelQueUpdate);
            var incrementUpdateResult = _FuelQue.UpdateOne(fuelStationFilter, FuelQueIncrementUpdate);

            // Return update status
            if (FuelQueUpdateResult == null || incrementUpdateResult == null) {
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
            var FuelQue = _FuelQue.Find(FuelQue => FuelQue.FuelStationId == fuelStation).FirstOrDefault();

            // Making customers list in the fuel queue
            List<QueueCustomer> queueCustomers = FuelQue.Customers.ToList();

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

            // Filtering FuelQue using fuel station id
            var fuelStationFilter = Builders<FuelQue>
             .Filter.Eq(e => e.FuelStationId, fuelStation);

            // Setting queue customers array
            var FuelQueUpdate = Builders<FuelQue>.Update.Set("customers", queueCustomers);

            // Decrementing total number of vehicles in the queue
            var FuelQueDecrementUpdate = Builders<FuelQue>.Update
                   .Inc(e => e.NumberOfVehicles, -1);

            // Commiting updates to database
            var FuelQueUpdateResult = _FuelQue.UpdateOne(Builders<FuelQue>
             .Filter.Eq(e => e.FuelStationId, fuelStation), FuelQueUpdate);
            var decrementUpdateResult = _FuelQue.UpdateOne(fuelStationFilter, FuelQueDecrementUpdate);

            // Return update status
            if (FuelQueUpdateResult == null || decrementUpdateResult == null)
            {
                return false;
            }

            return true;
        }

        /*
         * Function - Register fuel queue to the fuel station
         * Params - FuelQue(FuelQue) - FuelQue object to register
         * Returns - registered fuel queue(FuelQue)
         */
        public FuelQue Create(FuelQue FuelQue)
        {
            _FuelQue.InsertOne(FuelQue);
            return FuelQue;
        }

        /*
        * Function - Deleting fuel queue
        * Params - id(string) - fuel queue id to remove
        * Returns - void
        */
        public void Delete(string id)
        {
            _FuelQue.DeleteOne(FuelQue => FuelQue.Id == id);
        }

        /*
        * Function - Retrieving fuel queues
        * Params - no params
        * Returns - List<FuelQue> list of fuel queue objects
        */
        public List<FuelQue> Get()
        {
            return _FuelQue.Find(FuelQue => true).ToList();
        }

        /*
        * Function - Retrieving fuel queue
        * Params - id(string) - fuel queue id to retrive
        * Returns - FuelQue FuelQue object associated with id
        */
        public FuelQue Get(string id)
        {
            return _FuelQue.Find(FuelQue => FuelQue.Id == id).FirstOrDefault();
        }

        /*
        * Function - Retrieving fuel queue by fuel station id
        * Params - id(string) - fuel station id to retrive
        * Returns - FuelQue FuelQue object associated with id
        */
        public FuelQue GetFuelQueByFuelStationId(string id)
        {
            return _FuelQue.Find(FuelQue => FuelQue.FuelStationId == id).FirstOrDefault();
        }

        /*
        * Function - Updating fuel queue
        * Params - id(string) - fuel queue id to retrive
        * Returns - FuelQue fuel queue object associated with id
        */
        public void Update(string id, FuelQue FuelQue)
        {
            _FuelQue.ReplaceOne(FuelQue => FuelQue.Id == id, FuelQue);
        }
    }
}