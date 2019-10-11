using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimaTempoAPI.Models.Days
{
    public class DaysResponse
    {
        public DateTime Date { get; set; }
        public ClimateResponse Status { get; set; }

    }
}
