using equeue_server.Models;
using equeue_server.Models.Database;
using MongoDB.Driver;
using static System.Collections.Specialized.BitVector32;

/*
* FuleStationService: class Implements IFuleStationService: interface - Manages fuel station operations on database
*/
namespace equeue_server.Services
{
    public class FuelStationService : IFuelStationService
    {
        // variable to hold mongodb collection
        private readonly IMongoCollection<FuelStation> _fuelStation;

        // constructor - retrives collections and assign collection to _fuelStation
        public FuelStationService(IStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _fuelStation = database.GetCollection<FuelStation>(settings.StationCollectionName);
        }

        /*
        * Function - Register fuel station
        * Params - fuelStation(FuelStation) - FuelStation object to register
        * Returns - registered fuel station(FuelStation)
        */
        FuelStation IFuelStationService.Create(FuelStation fuelStation)
        {
            _fuelStation.InsertOne(fuelStation);
            return fuelStation;
        }

        /*
        * Function - Deleting fuel station
        * Params - id(string) - fuel station id to remove
        * Returns - void
        */
        void IFuelStationService.Delete(string id)
        {
            _fuelStation.DeleteOne(fuelStation => fuelStation.Id == id);
        }

        /*
        * Function - Retrieving fuel stations
        * Params - no params
        * Returns - List<FuelStation> list of fuel queue objects
        */
        List<FuelStation> IFuelStationService.Get()
        {
            return _fuelStation.Find(fuelStation => true).ToList();
        }

        /*
        * Function - Retrieving fuel stations of owner
        * Params - id(string) - owner id to retrive
        * Returns - List<FuelStation> list of fuel queue objects of certain owner
        */
        List<FuelStation> IFuelStationService.GetOwnerStations(string id)
        {
            return _fuelStation.Find(fuelStation => fuelStation.OwnerId == id).ToList();
        }


        /*
        * Function - Retrieving fuel station
        * Params - id(string) - fuel station id to retrive
        * Returns - FuelStation fuelStation object associated with id
        */
        FuelStation IFuelStationService.Get(string id)
        {
            return _fuelStation.Find(fuelStation => fuelStation.Id == id).FirstOrDefault();
        }

        /*
        * Function - Updating fuel station
        * Params - id(string) - fuel station id to retrive
        * Returns - FuelStation fuel station object associated with id
        */
        void IFuelStationService.Update(string id, FuelStation fuelStation)
        {
            _fuelStation.ReplaceOne(fuelStation => fuelStation.Id == id, fuelStation);

        }
    }
}