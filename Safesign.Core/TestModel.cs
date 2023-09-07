using System.Runtime.CompilerServices;
using Newtonsoft.Json;


public class TestModel {

    [JsonProperty]
    public string id { get; set; }
    public float x {get; set;}
    public float y {get; set;}
    public float z {get; set;}


    public TestModel(string _id, float _x, float _y, float _z) {
        id = _id;
        x = _x;
        y = _y;
        z = _z;
    }
}