using System.Runtime.CompilerServices;
using Newtonsoft.Json;


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
public class xyzData {

    public string id {get; set;}
    public float x {get; set;}
    public float y {get; set;}
    public float z {get; set;}
}