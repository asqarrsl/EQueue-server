using System;

/*
* IStoreDatabaseSettings: interface - Interfaces for manage database configs and collection names
*/
namespace equeue_server.Models.Database
{
    public interface IStoreDatabaseSettings
    {
        string UserCollectionName { get; set; }
        string FuelQueueCollectionName { get; set; }
        string StationCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

