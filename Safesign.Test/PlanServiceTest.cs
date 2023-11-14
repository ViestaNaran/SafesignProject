using Safesign.Data;
using Safesign.Api;
using Safesign.Core;
using Safesign.Services;
using Microsoft.Azure.Cosmos;



namespace Safesign.Test;

public class PlanServiceTest {

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
            SensorContainer = "UnitTestSigns",
            PlanContainer = "PlanUnitTest"
        };
        SignService signService = new SignService(connection);

        PlanService planService = new PlanService(connection, signService);
        
        int expectedCount = 4;
        
        // Act
        var plans = await planService.GetAll();

        // Assert
        Assert.NotNull(plans); // Check if the returned list is not null.
        Assert.NotEmpty(plans); // Check if the returned list is not empty.
        Assert.Equal(expectedCount, plans.Count); // Replace `expectedCount` with the expected number of signs.

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
            SensorContainer = "UnitTestSigns",
            PlanContainer = "PlanUnitTest"
        };

        SignService signService = new SignService(connection);
        PlanService planService = new PlanService(connection, signService);
        int expectedCount = 2;
        // Act
        var plans = await planService.GetAll();

        // Assert
        Assert.NotNull(plans); 
        Assert.NotEmpty(plans); 
        Assert.NotEqual(expectedCount, plans.Count);

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
            SensorContainer = "UnitTestSigns",
            PlanContainer = "PlanUnitTest"
        };

        SignService signService = new SignService(connection);
        PlanService planService = new PlanService(connection, signService);

        string planId = "plan1";
        // Act
        var plan = await planService.Get(planId);

        // Assert
        Assert.NotNull(plan); 
        Assert.Equal(plan.Id, planId);
    }

    [Fact]
    public async void Test_GetBySignId_Fail() {
        CosmosConnection connection = new CosmosConnection {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "UnitTestDB",
            SignContainer = "SignUnitTest",
            SensorContainer = "UnitTestSigns",
            PlanContainer = "PlanUnitTest"
        };

        SignService signService = new SignService(connection);
        PlanService planService = new PlanService(connection, signService);
        
        string planId = "plan52334234";
        
        // Act
        var sign = await planService.Get(planId);

        // Assert
        Assert.Null(sign); // Check if the returned list is not null.
    }

    [Fact]
    public async Task AddPlan_Should_Succeed()
    {
        // Arrange
        CosmosConnection connection = new CosmosConnection {
            EndpointUri = "https://localhost:8081",
            PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            SafesignDB = "SafesignUnitTest",
            TestDB = "UnitTestDB",
            SignContainer = "SignUnitTest",
            SensorContainer = "UnitTestSigns",
            PlanContainer = "PlanUnitTest"
        };

        SignService signService = new SignService(connection);
        PlanService planService = new PlanService(connection, signService);
        
        var client = new CosmosClient(connection.EndpointUri,connection.PrimaryKey);
        var db = client.GetDatabase(connection.SafesignDB);

        Container _planContainer = db.GetContainer(connection.PlanContainer);

        Random random = new Random();
        string planId = random.NextInt64().ToString();

        Plan planToAdd = new Plan {
            Id = planId,
            CSId = "cs123",
            Responsible = "Mr. Responsivery"
        };

        // Act
        var addedPlan = await planService.Add(planToAdd);

        // Assert
        Assert.NotNull(addedPlan);
        Assert.Equal(addedPlan.Id, planId);
        
        // Cleanup
        await _planContainer.DeleteItemAsync<Plan>(planId, new PartitionKey(planId));
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
















}