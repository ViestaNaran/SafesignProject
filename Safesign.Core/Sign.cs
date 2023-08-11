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
        public string Id { get;set; }
        public string ProjectId { get; set; } 
        // public float Angle { get; set; }

        // change these into GeoCoordinator class later?
        //  public double Lat { get; set; }
        // public double Long { get; set; }
    
        public Sign(string id, string projectId) {
            Id = id;
            ProjectId = projectId;
        }
    }
}
