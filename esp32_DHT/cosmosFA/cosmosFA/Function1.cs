using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace cosmosFA
{
    public static class saveToCosmos
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("saveToCosmos")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "iotHub", ConsumerGroup="cosmos")]EventData message, 
            [CosmosDB(
                databaseName: "cosmosatma",
                collectionName: "Messages",
                ConnectionStringSetting = "cosmosDbConnection",
                CreateIfNotExists = true
            )]out dynamic cosmos, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
            cosmos = Encoding.UTF8.GetString(message.Body.Array);
        }
    }
}