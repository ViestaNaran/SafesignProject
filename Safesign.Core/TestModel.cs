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

    // public TestModel(string _id, float _x, float _y, float _z) {
    //     id = _id;
    //     x = _x;
    //     y = _y;
    //     z = _z;
    // }
}

public class SensorData
{
    public string msg { get; set; }
    public List<TestModel> obj { get; set; }
    public string gmac { get; set; }
}

public class InternalSensorData {


}