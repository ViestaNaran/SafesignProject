using Newtonsoft.Json;

namespace Safesign.Core
{
    public class ConstructionSite
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string PlanId { get; set; }
        public string City { get; set; }
        public string Street { get; set;}
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        
        // False = problem, True = No Problem
        public bool Status {get; set;}
        public string MainResponsible { get; set; }
    }
}
