using Newtonsoft.Json;
using System;
using System.Net;

namespace ClimaTempoAPI.Models.Current
{
    public class CityResponse
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public CityClimate Data { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Detail { get; set; }

    }

    public class CityClimate
    {
        public double Temperature { get; set; }

        public string Wind_Direction { get; set; }

        public double Wind_Velocity { get; set; }

        public double Humidity { get; set; }

        public string Condition { get; set; }

        public double Pressure { get; set; }

        public double Sensation { get; set; }

        public DateTime Date { get; set; }
    }
}