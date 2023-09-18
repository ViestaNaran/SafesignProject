namespace Safesign.Core {

    // Variables do not follow standard naming conventions as to not interfere with the format from the sensor. 
    // Will fix in the future
    public class SensorData
    {
        public string msg { get; set; }
        public List<TestModel> obj { get; set; }
        public string gmac { get; set; }
    }
}