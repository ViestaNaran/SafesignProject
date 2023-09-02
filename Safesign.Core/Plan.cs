using Newtonsoft.Json;

namespace Safesign.Core
{
    public class Plan
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string CSId {get; set; }
        
        //public List<Sign> Signs {get; set;}
        //public MainReSponsible? MResponsible { get; set; }
        public string? Responsible { get; set; }

        public Plan() {}

        public Plan(string id, string csId, string responsible) {
            Id = id;
            CSId = csId;
            Responsible = responsible;
        }
    }
}
