using Newtonsoft.Json;

namespace Safesign.Core
{
    public class ConstructionSite
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        // public string Address { get; set; }
        public string PlanId { get; set; }
        public string City { get; set; }
        public string street { get; set;}
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string MainResponsible { get; set; }
        // public List<string> SignIds { get; set; }
        // public string Status { get; set; }
    }
}
