using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using cosmosFA.Models;
using System;


namespace cosmosFA
{
    public static class saveToCosmos
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("saveToCosmos")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "iotHub", ConsumerGroup = "cosmos")] EventData message,
            [CosmosDB(
                databaseName: "atcosmos",
                collectionName: "Messages",
                ConnectionStringSetting = "cosmosDbConnection",
                CreateIfNotExists = true
            )]out dynamic cosmos, ILogger log)
            {            
                var _deviceId = message.SystemProperties["iothub-connection-device-id"].ToString();            
                var _measurementType = message.Properties["measurementType"].ToString();
                var _school = message.Properties["School"].ToString();
                var _student = message.Properties["Student"].ToString();
                var _data = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(message.Body.Array));
                DateTime _measurementTime;
                /*if (_deviceId == "esp32")
                {*/
                    long unixTime = _data["epochTime"];
                    _measurementTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime; 
                /*}

                else 
                {
                    _measurementTime = _data["Now"];
                }*/
                var _cosmos = new CosmosDataModel()
                {
                    data = _data,
                    deviceId = _deviceId,
                    measurementType = _measurementType,
                    measurementTime = _measurementTime,
                    school = _school,
                    student = _student
                };
                cosmos = JsonConvert.SerializeObject(_cosmos);
                log.LogInformation($"Message in Cosmos DB: {JsonConvert.SerializeObject(_cosmos)}");       
        }
    }
}