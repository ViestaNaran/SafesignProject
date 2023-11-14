using System;
using System.ComponentModel;
using System.Data.Common;
using System.Net;
using NuGet.Frameworks;
using Safesign.Core;
using Safesign.Data;
using Safesign.Services;
using Microsoft.Azure.Cosmos;

namespace Safesign.Test;

public class SenSorServiceTest 
{

    [Fact]
    public async void Test_Get() {

        // Arrange
        CosmosConnection connection = new CosmosConnection
        {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "SafesignUnitTest",
            SignContainer = "SignUnitTest",
            SensorContainer = "TestModelUnitTest"
        };

        SignService signService = new SignService(connection);

        SensorService sensorService = new SensorService(connection, signService);

        string lookupId = "BC5729036D8C";        
        
        // Act
        
        var result = await sensorService.Get(lookupId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(lookupId, result.dmac);

    }

    [Fact]
    public async void Test_CreateRandom2_UpdateTestModel() {
        
        // Arrange
        CosmosConnection connection = new CosmosConnection
        {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "SafesignUnitTest",
            SignContainer = "SignUnitTest",
            SensorContainer = "TestModelUnitTest"
        };

        SignService signService = new SignService(connection);

        SensorService sensorService = new SensorService(connection,signService);

        var client = new CosmosClient(connection.EndpointUri,connection.PrimaryKey);
        var db = client.GetDatabase(connection.TestDB);

        Microsoft.Azure.Cosmos.Container _sensorContainer = db.GetContainer(connection.SensorContainer);

        string testModelDmac = "BC5729036D8C";

        TestModel model1 = new TestModel
        {
            dmac = "BC5729036D8C",
            x0 = 20.5f,
            y0 = 30.0f,
            z0 = 33.0f,
            type = 1
        };

        List<TestModel> testList = new List<TestModel> {
            model1
        };

        // gmac is a value that came from the sensor and needed to be included in the datamodel.
        // The SensorData class was implemented to receive the immediate data from the sensor. 
        SensorData testData = new SensorData {
            msg = "advData",
            obj = testList,
            gmac = "TestValue"
        }; 

        var compareObject = await sensorService.Get(testModelDmac);

        // Act
        var response = await sensorService.CreateRandom2(testData);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(testModelDmac, response.dmac);
        Assert.NotEqual(compareObject.x0, response.x0);
        Assert.NotEqual(compareObject.y0, response.y0);
        Assert.NotEqual(compareObject.z0, response.z0);

        Assert.Equal(model1.x0, response.x0);
        Assert.Equal(model1.y0, response.y0);
        Assert.Equal(model1.z0, response.z0);

        // cleanup
       TestModel cleanupModel = new TestModel
        {
            dmac = "BC5729036D8C",
            x0 = 10.5f,
            y0 = 20.7f,
            z0 = 15.2f,
            type = 1
        };

        await _sensorContainer.ReplaceItemAsync<TestModel>(cleanupModel, cleanupModel.dmac, new PartitionKey(cleanupModel.dmac));
    }

    [Fact]
    public async void Test_CreateRandom2_CreateNewTestModel() {
        
        // Arrange
        CosmosConnection connection = new CosmosConnection
        {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "SafesignUnitTest",
            SignContainer = "SignUnitTest",
            SensorContainer = "TestModelUnitTest"
        };

        SignService signService = new SignService(connection);

        SensorService sensorService = new SensorService(connection,signService);

        var client = new CosmosClient(connection.EndpointUri,connection.PrimaryKey);
        var db = client.GetDatabase(connection.TestDB);

        Microsoft.Azure.Cosmos.Container _sensorContainer = db.GetContainer(connection.SensorContainer);

        TestModel model1 = new TestModel
        {
            dmac = "sensor4",
            x0 = 20.5f,
            y0 = 30.0f,
            z0 = 33.0f,
            type = 1
        };

        List<TestModel> testList = new List<TestModel> {
            model1
        };

        // gmac is a value that came from the sensor and needed to be included in the datamodel.
        // The SensorData class was implemented to receive the immediate data from the sensor. 
        SensorData testData = new SensorData {
            msg = "advData",
            obj = testList,
            gmac = "TestValue"
        }; 

        // Act
        var response = await sensorService.CreateRandom2(testData);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(model1.x0, response.x0);
        Assert.Equal(model1.y0, response.y0);
        Assert.Equal(model1.z0, response.z0);

        // cleanup
        await _sensorContainer.DeleteItemAsync<TestModel>(model1.dmac, new PartitionKey(model1.dmac));
    }

    [Fact]
    public async void Test_UpdateSensorData() {
        
        // Arrange
        CosmosConnection connection = new CosmosConnection
        {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "SafesignUnitTest",
            SignContainer = "SignUnitTest",
            SensorContainer = "TestModelUnitTest"
        };

        SignService signService = new SignService(connection);

        SensorService sensorService = new SensorService(connection,signService);

        var client = new CosmosClient(connection.EndpointUri,connection.PrimaryKey);
        var db = client.GetDatabase(connection.TestDB);

        TestModel model1 = new TestModel
        {
            dmac = "sensor4",
            x0 = 20.5f,
            y0 = 30.0f,
            z0 = 33.0f,
            type = 1
        };

        List<TestModel> testList = new List<TestModel> {
            model1
        };

        // gmac is a value that came from the sensor and needed to be included in the datamodel.
        // The SensorData class was implemented to receive the immediate data from the sensor. 
        SensorData testData = new SensorData {
            msg = "advData",
            obj = testList,
            gmac = "TestValue"
        }; 

        Sign testSign = new Sign
        {
            Id = "12345",
            CSId = "CS001",
            PlanId = "Plan001",
            OgAngle = 45.5f,
            CurrAngle = 45.5f,
            Issue = "OK",
            SensorId = "sensor4",
            Type = 1,
            OgX = 10.5f,
            CurrX = 10.5f,
            OgY = 20.5f,
            CurrY = 20.5f,
            OgZ = 15.0f,
            CurrZ = 15.0f
        };

        await signService.Add(testSign);

        // Act
        var response = await sensorService.UpdateSensorData(testData);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(model1.x0, response.CurrX);
        Assert.Equal(model1.y0, response.CurrY);
        Assert.Equal(model1.z0, response.CurrZ);

        // cleanup
        await signService.Delete(testSign.Id);
    }
}