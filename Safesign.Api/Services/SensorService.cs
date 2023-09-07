using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Safesign.Core;
using Safesign.Data;


namespace Safesign.Services;

public class SensorService
{
    private readonly Container _sensorContainer;
    
    public SensorService(CosmosConnection connection)
    {
        var client = new CosmosClient(connection.EndpointUri, connection.PrimaryKey);
        var db = client.GetDatabase(connection.TestDB);
        _sensorContainer = db.GetContainer(connection.SensorContainer);
    }

    public async Task<TestModel> CreateRandom1(JsonObject randomObject) {
    
    
    Console.WriteLine(randomObject);
    
    float xValue = (float)randomObject["x0"];
    float yValue = (float)randomObject["y0"];
    float zValue = (float)randomObject["z0"];
    string id = (string)randomObject["dmac"];

    Console.WriteLine($"x:{xValue}, y: {yValue}, z: {zValue}, id: {id}");

    TestModel TestObject = new TestModel(id, xValue, yValue, zValue);
    
    Console.WriteLine($"x:{TestObject.x}, y: {TestObject.y}, z: {TestObject.z}, id: {TestObject.id}");

    return await _sensorContainer.CreateItemAsync<TestModel>(TestObject);       
}


}