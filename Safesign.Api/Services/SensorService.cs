using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Azure.Cosmos;
using Safesign.Core;
using Safesign.Data;


namespace Safesign.Services;

public class SensorService
{
    private readonly Container _sensorContainer;
    private readonly Container _signContainer;
    private readonly SignService _signService;
    
    public SensorService(CosmosConnection connection, SignService signService)
    {
        var client = new CosmosClient(connection.EndpointUri, connection.PrimaryKey);
        var db = client.GetDatabase(connection.TestDB);
        _sensorContainer = db.GetContainer(connection.SensorContainer);
        _signContainer = db.GetContainer(connection.SignContainer);
        _signService = signService;
    }

    public async Task<TestModel> Get(string macId) {
       
       var sensorData = _sensorContainer.GetItemLinqQueryable<TestModel>(true)
        .Where(p => p.dmac == macId)
        .AsEnumerable()
        .FirstOrDefault();

        return sensorData;
    }


    public async Task<TestModel> CreateRandom2(SensorData sensorData)
    {
        Console.WriteLine("CreateRandom2 called");
        Console.WriteLine(sensorData + "\n");

        if (sensorData.msg == "advData" && sensorData.obj.Count > 0 && sensorData.obj[0].type == 1)
        {
            TestModel reading = sensorData.obj[0];

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

    public async Task<Sign> UpdateSensorData(SensorData sensorData)
        {
      
        if (sensorData.msg == "advData" && sensorData.obj.Count > 0 && sensorData.obj[0].type == 1)
        {
            TestModel reading = sensorData.obj[0];

            var signToUpdate = await _signService.GetSignBySensorMac(reading.dmac);
            
            await _sensorContainer.ReplaceItemAsync<TestModel>(reading, reading.dmac, new PartitionKey(reading.dmac));

            //  Case: Sensor with this id exists, Update the sign associated with the sensor in the database.
            if (signToUpdate != null)
            {    
                signToUpdate.CurrX = reading.x0;
                signToUpdate.CurrY = reading.y0;
                signToUpdate.CurrZ = reading.z0;

                return await _signService.Update(signToUpdate.Id, signToUpdate);
            }
            // Case: The sensor is not equipped to any sign, return null. 
            else
            {    
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}