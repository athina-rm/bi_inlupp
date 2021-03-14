using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace cosmosFA
{
    public static class dataRetrieval
    {
        [FunctionName("dataRetrieval")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName:"atcosmos",
                collectionName:"Messages",
                ConnectionStringSetting = "cosmosDbConnection",
                SqlQuery="SELECT TOP 10 * FROM c ORDER BY c._ts DESC"
            )]IEnumerable<dynamic>cosmos, ILogger log)
        {
            log.LogInformation("HTTP trigger function processed a request.");

            return new OkObjectResult(cosmos);
        }
    }
}
