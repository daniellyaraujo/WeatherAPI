using Newtonsoft.Json;
using System;
using System.Net;

namespace ClimaTempoAPI.Models.Days
{
    public class DaysResponse
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
        public DateTime? Date { get; set; }
        public ClimateResponse Status { get; set; }
        public string  Detail { get; set; }
    }
}
