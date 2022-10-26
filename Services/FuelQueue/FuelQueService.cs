/*
* FuelQueueService: class Implements IFuelQueueService: interface - fuel queue operations on database are managed
*/

using equeue_server.Models.Database;
using equeue_server.Models;
using MongoDB.Driver;


namespace equeue_server.Services
{
    public class FuelQueService : IFuelQueService
    {
        // variable to hold mongodb collection
        private readonly IMongoCollection<FuelQue> _fuelQue;

        // constructor - retrives collections and assign collection to _fuelQue
        public FuelQueService(IStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            var db = mongoClient.GetDatabase(settings.DatabaseName);
            _fuelQue = db.GetCollection<FuelQue>(settings.FuelQueueCollectionName);
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
            var fuelQueueUpdateResult = _fuelQue.UpdateOne(fuelStationFilter, fuelQueueUpdate);
            var incrementUpdateResult = _fuelQue.UpdateOne(fuelStationFilter, fuelQueueIncrementUpdate);

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
            var fuelQueue = _fuelQue.Find(fuelQueue => fuelQueue.FuelStationId == fuelStation).FirstOrDefault();

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
            var fuelQueueUpdateResult = _fuelQue.UpdateOne(Builders<FuelQue>
             .Filter.Eq(e => e.FuelStationId, fuelStation), fuelQueueUpdate);
            var decrementUpdateResult = _fuelQue.UpdateOne(fuelStationFilter, fuelQueueDecrementUpdate);

            // Return update status
            if (fuelQueueUpdateResult == null || decrementUpdateResult == null)
            {
                return false;
            }

            return true;
        }

        /*
         * Function - Register fuel queue to the fuel station
         * Params - fuelQueue(FuelQueue) - FuelQueue object to register
         * Returns - registered fuel queue(FuelQueue)
         */
        public FuelQue Create(FuelQue fuelQueue)
        {
            _fuelQue.InsertOne(fuelQueue);
            return fuelQueue;
        }

        /*
        * Function - Deleting fuel queue
        * Params - id(string) - fuel queue id to remove
        * Returns - void
        */
        public void Delete(string id)
        {
            _fuelQue.DeleteOne(fuelQueue => fuelQueue.Id == id);
        }

        /*
        * Function - Retrieving fuel queues
        * Params - no params
        * Returns - List<FuelQueue> list of fuel queue objects
        */
        public List<FuelQue> Get()
        {
            return _fuelQue.Find(fuelQueue => true).ToList();
        }

        /*
        * Function - Retrieving fuel queue
        * Params - id(string) - fuel queue id to retrive
        * Returns - FuelQueue fuelQueue object associated with id
        */
        public FuelQue Get(string id)
        {
            return _fuelQue.Find(fuelQueue => fuelQueue.Id == id).FirstOrDefault();
        }

        /*
        * Function - Updating fuel queue
        * Params - id(string) - fuel queue id to retrive
        * Returns - FuelQueue fuel queue object associated with id
        */
        public void Update(string id, FuelQue fuelQueue)
        {
            _fuelQue.ReplaceOne(fuelQueue => fuelQueue.Id == id, fuelQueue);
        }
    }
}
