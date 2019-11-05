using Newtonsoft.Json;
using System;

namespace ClimaTempoAPI.Models.Days
{
    public class ClimateResponse
    {
        public DateTime? Data { get; set; }

        //[JsonProperty("Umidade")]
        public Temperature Humidity { get; set; }

        //[JsonProperty("Sensação Termica")]
        public Temperature Thermal_Sensation { get; set; }

        //[JsonProperty("Temperatura")]
        public Temperature Temperature { get; set; }
    }
}