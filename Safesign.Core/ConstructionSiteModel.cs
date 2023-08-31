using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Safesign.Core
{
    public class ConstructionSiteModel
    {
        public  ConstructionSite CSSite {get; set; }
        public List<Sign> Signs {get; set; }
    }
}
