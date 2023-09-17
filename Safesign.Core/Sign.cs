using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace Safesign.Core
{
    public class Sign
    {
        [JsonProperty("id")] 
        public string Id { get; set; }
        public string CSId {get; set; }
        public string PlanId { get; set; }
        public float OgAngle {get; set; }
        public float CurrAngle {get; set; }
        public string Issue {get; set;}
        // Sensor reading values
        [JsonProperty("dmac")]
        public string SensorId {get; set;}

        [JsonProperty("type")]
        public int Type {get; set;}

        //[JsonProperty("z0")]
        public float OgX {get; set; }
        public float CurrX {get; set;}
        
        //[JsonProperty("y0")]
        public float OgY {get; set; }
        public float CurrY {get; set; }

        //[JsonProperty("x0")]
        public float OgZ {get; set; }
        public float CurrZ {get; set; }
        
        public Sign () {}
        public Sign(string id, string csId, string planId, float ogAngle) {
            Id = id;
            CSId = csId;
            PlanId = planId;
            OgAngle = ogAngle;
            CurrAngle = ogAngle;
        }

        public Sign(string id, string csId, string planId, string sensorId) {
            Id = id;
            CSId = csId;
            PlanId = planId;
            SensorId = sensorId;
        }
    }
}
