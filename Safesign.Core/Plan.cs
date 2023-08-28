using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Safesign.Core
{
    public class Plan
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string CSId {get; set; }
        public List<Sign> Signs {get; set;}
       //public MainReSponsible? MResponsible { get; set; }
        public string? Responsible { get; set; }

        public Plan() {}

        public Plan(string id, string responsible) {
            Id = id;
            Responsible = responsible;
        }
    }
}
