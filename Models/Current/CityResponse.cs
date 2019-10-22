using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace ClimaTempoAPI.Models.Current
{
    public class CityResponse
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Detail { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double Temperature { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string WindDirection { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double WindVelocity { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Humidity { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Condition { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Pressure { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Sensation { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Date { get; set; }

    }

    public class CityClimate
    {
        public List<CityResponse> Data { get; set; }
    }
}
