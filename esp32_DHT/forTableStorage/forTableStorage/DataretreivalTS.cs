using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using forTableStorage.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace forTableStorage
{
    public static class DataretreivalTS
    {
        [FunctionName("DataretreivalTS")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get",  Route = null)] HttpRequest req,
            [Table("fortablestorage")]CloudTable cloudTable,
            ILogger log)
        {
            IEnumerable<dataModel> results = await cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<dataModel>(), null);
            
                results.OrderByDescending(ts => ts.MeasurementTime);            
                results = results.Take(10);
            return new OkObjectResult(results);
        }
    }
}

