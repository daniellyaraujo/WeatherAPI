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

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Detail { get; set; }
    }

    public class Hour
    {
        public DateTime Date_br { get; set; }
        public Rain rain { get; set; }
    }

    public class Rain
    {
        public double Precipitation { get; set; }
        public Wind wind { get; set; }
    }

    public class Wind
    {
        public double Velocity { get; set; }
        public int Direction { get; set; }
        //diração graus
        public int DirectionDegrees { get; set; }
        //rajada
        public double gust { get; set; }

        public TemperatureClimate Temperature { get; set; }
    }
    public class TemperatureClimate
    {
        public double Temperature { get; set; }
    }
}