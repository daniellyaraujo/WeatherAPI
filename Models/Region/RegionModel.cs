using System.Collections.Generic;

namespace ClimaTempoAPI.Models.Region
{
    public class RegionModel
    {
        public List<string> RegionModels { get; }

        public RegionModel()
        {
            RegionModels = new List<string>();

            RegionModels.Add("sul");
            RegionModels.Add("sudeste");
            RegionModels.Add("norte");
            RegionModels.Add("nordeste");
            RegionModels.Add("centro oeste");
        }
    };
}
