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
        public Sign () {}
        public Sign(string id, string csId, string planId, float ogAngle) {
            Id = id;
            CSId = csId;
            PlanId = planId;
            OgAngle = ogAngle;
            CurrAngle = ogAngle;
        }
    }
}
