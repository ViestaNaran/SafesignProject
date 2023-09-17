using Newtonsoft.Json;

namespace Safesign.Core
{
    // violating naming rules here to load values properly from the sensor
    // fix in the future
    public class TestModel {

        [JsonProperty("id")]
        public string dmac { get; set; }
    
        [JsonProperty("x")]
        public float x0 {get; set;}
    
        [JsonProperty("y")]
        public float y0 {get; set;}
    
        [JsonProperty("z")]
        public float z0 {get; set;}
        public int type {get; set;}

    }
}