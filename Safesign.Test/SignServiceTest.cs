using System;
using System.Net;
using Safesign.Core;
using Safesign.Data;
using Safesign.Services;

namespace Safesign.Test;

public class SignServiceTest
{
    [Fact]
    public async void Test_GetAll_Success()
    {
        // Arrange
        CosmosConnection connection = new CosmosConnection {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "UnitTestDB",
            SignContainer = "SignUnitTest",
            SensorContainer = "UnitTestSigns"
        };

        SignService signService = new SignService(connection);
        int expectedCount = 3;
        // Act
        var signs = await signService.GetAll();

        // Assert
        Assert.NotNull(signs); // Check if the returned list is not null.
        Assert.NotEmpty(signs); // Check if the returned list is not empty.
        Assert.Equal(expectedCount, signs.Count); // Replace `expectedCount` with the expected number of signs.

    }

    [Fact]
    public async void Test_GetAll_Fail() {
        // Arrange
        CosmosConnection connection = new CosmosConnection {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "UnitTestDB",
            SignContainer = "SignUnitTest",
            SensorContainer = "UnitTestSigns"
        };

        SignService signService = new SignService(connection);
        int expectedCount = 2;
        // Act
        var signs = await signService.GetAll();

        // Assert
        Assert.NotNull(signs); 
        Assert.NotEmpty(signs); 
        Assert.NotEqual(expectedCount, signs.Count);

    }

    [Fact]
    public async void Test_GetBySignId_Success() {
        // Arrange
        CosmosConnection connection = new CosmosConnection {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "UnitTestDB",
            SignContainer = "SignUnitTest",
            SensorContainer = "UnitTestSigns"
        };

        SignService signService = new SignService(connection);
        string signId = "sign1";
        // Act
        var sign = await signService.Get(signId);

        // Assert
        Assert.NotNull(sign); 
        Assert.Equal(sign.Id, signId); 

    }

    [Fact]
    public async void Test_GetBySignId_Fail() {
        // Arrange
        CosmosConnection connection = new CosmosConnection {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "UnitTestDB",
            SignContainer = "SignUnitTest",
            SensorContainer = "UnitTestSigns"
        };

        SignService signService = new SignService(connection);
        string signId = "sign4";
        // Act
        var sign = await signService.Get(signId);

        // Assert
        Assert.Null(sign); // Check if the returned list is not null.
    }

    [Fact]
    public async void Test_GetSignsByPlanId_WithCorrectPlanId()
    {
        // Arrange
        CosmosConnection connection = new CosmosConnection {
                EndpointUri = "https://localhost:8081",
                PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                SafesignDB = "SafesignUnitTest",
                TestDB = "UnitTestDB",
                SignContainer = "SignUnitTest",
                SensorContainer = "UnitTestSigns"
            };
        
        var signService = new SignService(connection);

        string planIdToSearch = "plan3"; // Use an actual plan ID that exists in your test data.
        

        // Act
        var retrievedSigns = await signService.GetSignsByPlanId(planIdToSearch);

        // Assert
        Assert.NotNull(retrievedSigns);
        Assert.NotEmpty(retrievedSigns);
        Assert.Equal(retrievedSigns.FirstOrDefault().PlanId, planIdToSearch); // Expecting 2 signs with the specified plan ID.
    }

    [Fact]
    public async void Test_GetSignsByPlanId_WithIncorrectPlanId()
    {
        // Arrange
        
        CosmosConnection connection = new CosmosConnection {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "UnitTestDB",
            SignContainer = "SignUnitTest",
            SensorContainer = "UnitTestSigns"
        };
        var signService = new SignService(connection);
        string planIdToSearch = "NonExistentPlan"; // Use a plan ID that doesn't exist in your test data.
        // Act
        var retrievedSigns = await signService.GetSignsByPlanId(planIdToSearch);

        // Assert
        Assert.NotNull(retrievedSigns);
        Assert.Empty(retrievedSigns); // Expecting no signs for the incorrect plan ID.
    }
    
    // [Fact]
    // public async void Test_GetSignBySensorMac_WithCorrectMac()
    // {
    //     // Arrange
    //     CosmosConnection connection = new CosmosConnection {
    //             EndpointUri = "https://localhost:8081",
    //             PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
    //             SafesignDB = "SafesignUnitTest",
    //             TestDB = "UnitTestDB",
    //             SignContainer = "SignUnitTest",
    //             SensorContainer = "UnitTestSigns"
    //         };
        
    //     var signService = new SignService(connection);
    //     string sensorId = "sensor1";      

    //     // Act
    //     var retrievedSign = await signService.GetSignBySensorMac(sensorId);

    //     // Assert
    //     Assert.NotNull(retrievedSign);
    //     Assert.Equal(retrievedSign.SensorId, sensorId); // Expecting 2 signs with the specified plan ID.
    // }

    [Fact]
    public async void Test_GetSignsBySensorMac_WithIncorrectMac()
    {
        // Arrange
        CosmosConnection connection = new CosmosConnection {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "UnitTestDB",
            SignContainer = "SignUnitTest",
            SensorContainer = "UnitTestSigns"
        };
        var signService = new SignService(connection);
        
        string sensorId = "NonExistentSensor"; // Use a plan ID that doesn't exist in your test data.
        // Act
        
        var retrievedSign = await signService.GetSignBySensorMac(sensorId);

        // Assert
        Assert.Null(retrievedSign);
    }

    [Fact]
    public async Task AddSign_Should_Succeed()
    {
        // Arrange
      CosmosConnection connection = new CosmosConnection {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "UnitTestDB",
            SignContainer = "SignUnitTest",
            SensorContainer = "UnitTestSigns"
        };
        var signService = new SignService(connection);

        Random random = new Random();

        string signId = random.NextInt64().ToString();

        var signToAdd = new Sign
        {
            Id = signId,
            CSId = "cs1",
            PlanId = "plan1",
            OgAngle = 20,
            CurrAngle = 20,
            Issue = "None",
            SensorId = "sensor4",
            Type = 1,
            OgX = 10,
            OgY = 10,
            OgZ = 10,
        };

        // Act
        var addedSign = await signService.Add(signToAdd);

        // Assert
        Assert.NotNull(addedSign);
        Assert.Equal(addedSign.Id, signToAdd.Id);
        
        // Cleanup
        await signService.Delete(signId);
    }

    [Fact]
     public async void Test_Delete_ShouldSuceed()
    {
        // Arrange
        CosmosConnection connection = new CosmosConnection {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "UnitTestDB",
            SignContainer = "SignUnitTest",
            SensorContainer = "UnitTestSigns"
        };
        var signService = new SignService(connection);
        
        // Add a sign to be deleted
        Random random = new Random();

        string signId = random.NextInt64().ToString();

        var signToAdd = new Sign
        {
            Id = signId,
            CSId = "cs1",
            PlanId = "plan1",
            OgAngle = 20,
            CurrAngle = 20,
            Issue = "None",
            SensorId = "sensor4",
            Type = 1,
            OgX = 10,
            OgY = 10,
            OgZ = 10,
        };

        var signToDelete = await signService.Add(signToAdd);

        // Act
        var response = await signService.Delete(signToDelete.Id);        
        
        // Assert
        Assert.True(response);
    }

    [Fact]
    public async void Test_Delete_ShouldFail()
    {
        // Arrange
        CosmosConnection connection = new CosmosConnection {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "UnitTestDB",
            SignContainer = "SignUnitTest",
            SensorContainer = "UnitTestSigns"
        };
        var signService = new SignService(connection);
        
       string signNotPresentId = "1234";

        // Act
        var response = await signService.Delete(signNotPresentId);        
        
        // Assert
        Assert.False(response);
    }


    [Fact]
    public async Task Test_Update()
    {
        // Arrange
        CosmosConnection connection = new CosmosConnection
        {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "UnitTestDB",
            SignContainer = "SignUnitTest",
            SensorContainer = "UnitTestSigns"
        };

        var signService = new SignService(connection);
        
        Random random = new Random();

        string signId = random.NextInt64().ToString();

        var signToAdd = new Sign
        {
            Id = signId,
            CSId = "cs1",
            PlanId = "plan1",
            OgAngle = 20,
            CurrAngle = 20,
            Issue = "None",
            SensorId = "sensor4",
            Type = 1,
            OgX = 10,
            OgY = 10,
            OgZ = 10,
        };

        var addedSign = await signService.Add(signToAdd);

        // A sign to replace the "signToAdd" object in the datbase.
        // The value that is being updated is CurrAngle from 20 -> 40

        Sign dummySign = new Sign
        {
            Id = signId,
            CSId = "cs1",
            PlanId = "plan1",
            OgAngle = 20,
            CurrAngle = 40,
            Issue = "None",
            SensorId = "sensor4",
            Type = 1,
            OgX = 10,
            OgY = 10,
            OgZ = 10,
            // Set other properties as needed
        };

        // Assert that the added sign exists
        Assert.NotNull(addedSign);
        Assert.Equal(addedSign.Id, signToAdd.Id);

        // Act
        await signService.Update(addedSign.Id, dummySign);
        var updatedSign = await signService.Get(dummySign.Id);
        // Assert
        // Assert.NotNull(updatedSign);

        Assert.Equal(dummySign.Id, updatedSign.Id);
        Assert.Equal(dummySign.CurrAngle, updatedSign.CurrAngle);
        
        await signService.Delete(updatedSign.Id);
    }

    


}