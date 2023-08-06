using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Safesign.Core
{
    internal class Plan
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        public List<Sign> Signs { get; set; }
        public MainReSponsible? MResponsible { get; set; }
    
    }
}
