using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Safesign.Core
{
    public class ConstructionSite
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Address { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string MainResponsible { get; set; }
        public List<string> SignIds { get; set; }
        public string Status { get; set; }
    }
}
