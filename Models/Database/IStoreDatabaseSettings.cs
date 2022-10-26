/*
* IStoreDatabaseSettings: interface - Interfaces for managing database configs and collection names
*/

using System;


namespace equeue_server.Models.Database
{
    public interface IStoreDatabaseSettings
    {
        string UserCollectionName { get; set; }
        string StationCollectionName { get; set; }
        string FuelQueueCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

