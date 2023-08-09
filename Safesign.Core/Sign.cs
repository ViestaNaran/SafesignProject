using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safesign.Core
{
    public class Sign
    {
        [JsonProperty("id")]
        public int Id { get;set; }
        public int ProjectId { get; set; } 
        public float Angle { get; set; }

        // change these into GeoCoordinator class later?
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
