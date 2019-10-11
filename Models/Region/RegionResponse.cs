using System;
using System.Collections.Generic;

namespace WeatherAPI.Models.Region
{
    public class RegionResponse
    {
        public List<RegionRequest> Data { get; set; }
    }

    public class RegionRequest
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
