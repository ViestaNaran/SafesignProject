using Newtonsoft.Json;

namespace Safesign.Core
{
    public class Sign
    {
        [JsonProperty("id")]
       // public string Id { get;set; }
        
        public string Id { get; set; }
        public string ProjectId { get; set; } 
        // public float Angle { get; set; }
        
        // change these into GeoCoordinator class later?
        // public double Lat { get; set; }
        // public double Longtitude { get; set; }
        
        public float OgAngle {get; set; }
        public float CurrAngle {get; set; }
        public Sign () {}
        public Sign(string id, string projectId, float ogAngle) {
            Id = id;
            ProjectId = projectId;
            OgAngle = ogAngle;
            CurrAngle = ogAngle;
        }
    }
}
