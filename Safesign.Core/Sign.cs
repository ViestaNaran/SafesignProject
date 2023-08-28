using System.Security.Claims;
using Newtonsoft.Json;

namespace Safesign.Core
{
    public class Sign
    {
        [JsonProperty("id")] 
        public string Id { get; set; }
        public string CSId {get; set; }
        public string ProjectId { get; set; }
        public float OgAngle {get; set; }
        public float CurrAngle {get; set; }
        public Sign () {}
        public Sign(string id, string csId, string projectId, float ogAngle) {
            Id = id;
            CSId = csId;
            ProjectId = projectId;
            OgAngle = ogAngle;
            CurrAngle = ogAngle;
        }
    }
}
