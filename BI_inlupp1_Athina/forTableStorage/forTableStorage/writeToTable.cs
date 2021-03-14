using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using forTableStorage.Model;

namespace forTableStorage
{
    public static class writeToTable
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("writeToTable")]
        [return: Table("fortablestorage")]
        public static dataModel Run([IoTHubTrigger("messages/events", Connection = "iothubendpoint",ConsumerGroup = "storagecg")]EventData message, ILogger log)
            {
            try
            {
                var msg = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(message.Body.Array));
                long unixTime = msg["epochTime"];
                var payload = new dataModel()
                {
                    PartitionKey = message.SystemProperties["iothub-connection-device-id"].ToString(),
                    RowKey = Guid.NewGuid().ToString(),
                    MeasurementType = message.Properties["measurementType"].ToString(),
                    School = message.Properties["School"].ToString(),
                    Student = message.Properties["Student"].ToString(),
                    MeasurementTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime,
                    Temperature = msg["temperature"],
                    Humidity = msg["humidity"]                    
                };
                return payload;
            }
            catch {
                log.LogInformation("Couldn't deserialize the message");
            }

            return null;
        }
    }
}
