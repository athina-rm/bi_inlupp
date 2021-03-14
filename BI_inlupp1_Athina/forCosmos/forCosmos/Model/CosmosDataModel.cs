using System;
using System.Collections.Generic;
using System.Text;

namespace cosmosFA.Models
{
    class CosmosDataModel
    {
        public dynamic data { get; set; }
        public string deviceId { get; set; }
        public string measurementType { get; set; }
        public DateTime measurementTime { get; set; }
        public string school { get; set; }
        public string student { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

    }
}
