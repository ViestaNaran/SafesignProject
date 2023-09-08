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

    // public async Task<TestModel> CreateRandom1(JsonObject randomObject) {
    
    // Console.WriteLine(randomObject);

    // int type = (int)randomObject["type"];

    // if(type == 1) {
    //    // return null;
    
    //     float xValue = (float)randomObject["x0"];
    //     float yValue = (float)randomObject["y0"];
    //     float zValue = (float)randomObject["z0"];
    //     string id = (string)randomObject["dmac"];

    //     Console.WriteLine($"x:{xValue}, y: {yValue}, z: {zValue}, id: {id}");

    //     TestModel TestObject = new TestModel(id, xValue, yValue, zValue);
        
    //     Console.WriteLine($"x:{TestObject.x}, y: {TestObject.y}, z: {TestObject.z}, id: {TestObject.id}");

    //     var lookedUpModel = _sensorContainer.GetItemLinqQueryable<TestModel>(true)
    //         .Where(p => p.id == id)
    //         .AsEnumerable()
    //         .ToList();


    //     foreach(TestModel tm in lookedUpModel) {
    //         Console.WriteLine($"id: {tm.id}");
    //     }

    //     if(lookedUpModel.Count > 0) {
    //         return await _sensorContainer.ReplaceItemAsync<TestModel>(TestObject, id, new PartitionKey(id));
    //     } 
    //     else 
    //     {
    //         return await _sensorContainer.CreateItemAsync<TestModel>(TestObject);
    //     }
    //     } 
    //     else {return null;}
    // }

    public async Task<TestModel> CreateRandom2(SensorData sensorData)
    {
        Console.WriteLine("CreateRandom2 called");
        Console.WriteLine(sensorData + "\n");

        if (sensorData.msg == "advData" && sensorData.obj.Count > 0 && sensorData.obj[0].type == 1)
        {
            TestModel reading = sensorData.obj[0];

            // Extract the desired fields
            // string id = reading.id;
            // float xValue = reading.x;
            // float yValue = reading.y;
            // float zValue = reading.z;

           // Console.WriteLine($"x:{xValue}, y: {yValue}, z: {zValue}, id: {id}");

            //TestModel testObject = new TestModel(id, xValue, yValue, zValue);
 
 //           Console.WriteLine($"x:{testObject.x}, y: {testObject.y}, z: {testObject.z}, id: {testObject.id}");

            var lookedUpModel = _sensorContainer.GetItemLinqQueryable<TestModel>(true)
                .Where(p => p.dmac == reading.dmac)
                .AsEnumerable()
                .ToList();

            foreach (TestModel tm in lookedUpModel)
            {
                Console.WriteLine($"id: {tm.dmac}");
            }

            if (lookedUpModel.Count > 0)
            {
                return await _sensorContainer.ReplaceItemAsync<TestModel>(reading, reading.dmac, new PartitionKey(reading.dmac));
            }
            else
            {
                return await _sensorContainer.CreateItemAsync<TestModel>(reading);
            }
        }
        else
        {
            return null;
        }
    }
}