using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace T2SDataSync
{
    public static class Function2
    {
        private static readonly string _endpointUrl = "<COSMOSDB_URL>";
        private static readonly string _primaryKey = "<CONNECTION_STRING>";
        private static readonly string _databaseId = "database";
        private static readonly string _containerId = "collection2";
        private static CosmosClient cosmosClient = new CosmosClient(_endpointUrl, _primaryKey);
        [FunctionName("Function2")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "database",
            collectionName: "collection1",
            ConnectionStringSetting = "ConnectionString",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists =true)]IReadOnlyList<Document> input,
            ILogger log)
        {
            var container2 = cosmosClient.GetContainer(_databaseId, _containerId);

            await Task.Delay(60000);
            foreach (var doc in input)
            {
                try
                {
                    await container2.CreateItemAsync<Document>(doc);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
}
