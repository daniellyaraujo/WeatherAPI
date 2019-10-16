using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace WeatherAPI.Models.Region
{
    public class RegionResponse
    {
        public RegionResponse()
        {
            Data = new List<Region>();
        }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Detail { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<Region> Data { get; set; }
    }

    public class Region
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}