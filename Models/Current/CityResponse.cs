using System;
using System.Collections.Generic;

namespace ClimaTempoAPI.Models.Current
{
    public class CityResponse
    {
        public double Temperature { get; set; }
        public string WindDirection { get; set; }
        public double WindVelocity { get; set; }
        public int Humidity { get; set; }
        public string Condition { get; set; }
        public int Pressure { get; set; }
        public int Sensation { get; set; }
        public DateTime Date { get; set; }

    }

    public class CityClimate
    {
        public List<CityResponse> Data { get; set; }
    }
}
