
using Safesign.Core;

public class SensorData
{
    public string msg { get; set; }
    public List<TestModel> obj { get; set; }
    public string gmac { get; set; }
}


public class SensorData2
{
    public string msg { get; set; }
    public List<Sign> obj { get; set; }
    public string gmac { get; set; }
}