/*
* StoreDatabaseSettings: class Implements IStoreDatabaseSettings: interface - database configs and collection names are managed
*/

using System;


namespace equeue_server.Models.Database
{
    public class StoreDatabaseSettings : IStoreDatabaseSettings
    {
        public StoreDatabaseSettings() {}

        public string UserCollectionName { get; set; } = String.Empty;
        public string StationCollectionName { get; set; } = String.Empty;
        public string FuelQueueCollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}

