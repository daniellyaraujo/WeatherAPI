using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace ClimaTempoAPI.Models.Hour
{
    public class HourResponse
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
        public List<Hour> Data { get; set; }
        public string Detail { get; set; }
    }

    public class Hour
    {
        public DateTime Date { get; set; }
        public double Rain { get; set; }
    }

    public class WindRequest
    {
        public List<Wind> wind { get; set; }
    }
    public class Wind
    {
        public double Velocity { get; set; }
        public string Direction { get; set; }
    }

    public class TemperatureResponse
    {
        public List<TemperatureClimate> Temperature { get; set; }
    }

    public class TemperatureClimate
    {
        public double Climate { get; set; }
    }
}
