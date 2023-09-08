using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
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

    int type = (int)randomObject["type"];

    if(type == 1) {
       // return null;
    
        float xValue = (float)randomObject["x0"];
        float yValue = (float)randomObject["y0"];
        float zValue = (float)randomObject["z0"];
        string id = (string)randomObject["dmac"];

        Console.WriteLine($"x:{xValue}, y: {yValue}, z: {zValue}, id: {id}");

        TestModel TestObject = new TestModel(id, xValue, yValue, zValue);
        
        Console.WriteLine($"x:{TestObject.x}, y: {TestObject.y}, z: {TestObject.z}, id: {TestObject.id}");

        var lookedUpModel = _sensorContainer.GetItemLinqQueryable<TestModel>(true)
            .Where(p => p.id == id)
            .AsEnumerable()
            .ToList();


        foreach(TestModel tm in lookedUpModel) {
            Console.WriteLine($"id: {tm.id}");
        }

        if(lookedUpModel.Count > 0) {
            return await _sensorContainer.ReplaceItemAsync<TestModel>(TestObject, id, new PartitionKey(id));
        } else {
            return await _sensorContainer.CreateItemAsync<TestModel>(TestObject);
            }
        } else {return null;}
    }
}