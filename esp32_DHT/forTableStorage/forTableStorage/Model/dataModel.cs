using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace forTableStorage.Model
{
    public class dataModel : TableEntity
    {
        public string MeasurementType { get; set; }
        public DateTime MeasurementTime { get; set; }

        public double Temperature { get; set; }

        public int Humidity { get; set; }
    }
}
