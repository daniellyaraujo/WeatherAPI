using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace ClimaTempoAPI.Models.Days
{
    public class DaysResponse
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        public List<ClimateResponse> Data { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string  Detail { get; set; }
    }
}