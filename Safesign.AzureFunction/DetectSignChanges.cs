using Safesign.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;

namespace Safesign.AzureFunction
{
    public class DetectSignChanges
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly int angleOffSet = 5;
    
    
    // [Function("negotiate")]
    // public static HttpResponseData Negotiate([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequestData req,
    // [SignalRConnectionInfoInput(HubName = "serverless")] string connectionInfo)
    //     {
    //         var response = req.CreateResponse(HttpStatusCode.OK);
    //         response.Headers.Add("Content-Type", "application/json");
    //         response.WriteString(connectionInfo);
    //         return response;
    //     }

        public DetectSignChanges(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory) 
        {
            _logger = loggerFactory.CreateLogger<DetectSignChanges>();
            _httpClient = httpClientFactory.CreateClient("Api");
        }

        //[Function("DetectSignChanges")]
        // [Function(nameof(DetectAngleChange))]
        // [SignalROutput(HubName = "SignHub", ConnectionStringSetting = "AzureSignalRConnectionString")]
        // public async Task<SignalRMessageAction> DetectAngleChange(
        //     //[HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData requestData,
        //     [CosmosDBTrigger(
        //     databaseName: "Safesign",
        //     collectionName: "Signs",
        //     ConnectionStringSetting = "CosmosConnectionString", CreateLeaseCollectionIfNotExists = true,
        //     LeaseCollectionName = "SignLeases")] IReadOnlyList<Sign> input
        //     )
        // {
        //     _logger.LogInformation("DetectSignChanges running!");

        //     if(input != null && input.Count > 0) 
        //     {
        //         _logger.LogInformation($"Documents Modified: {input.Count}");
                
        //         foreach(var i in input) 
        //         {
        //             _logger.LogInformation($"SignModified: {i.Id}");
                    
        //             // Angle changed
        //             if(i.CurrAngle < i.OgAngle - angleOffSet || i.CurrAngle > i.OgAngle + angleOffSet) 
        //             {
        //                 _logger.LogInformation($"Sign {i.Id} in project {i.ProjectId} is angled incorrectly");
        //                 await Task.Run(() => {
        //                     return new SignalRMessageAction("SignAngleIssue", new object[] {i.Id,i.ProjectId,i.CurrAngle});
        //                 });
        //             }
        //         }
        //     }
        //     return null;
        // }

        [Function(nameof(DetectAngleChange2))]
        [SignalROutput(HubName = "serverless")]
        public async Task<SignalRMessageAction> DetectAngleChange2(
            //[HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData requestData,
            [CosmosDBTrigger(
            databaseName: "Safesign",
            collectionName: "Signs",
            ConnectionStringSetting = "CosmosConnectionString", CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "SignLeases")] IReadOnlyList<Sign> input
            )
        {
            _logger.LogInformation("DetectSignChanges running!");

            if(input != null && input.Count > 0) 
            {
                _logger.LogInformation($"Documents Modified: {input.Count}");
                
                foreach(var i in input) 
                {
                    _logger.LogInformation($"SignModified: {i.Id}");
                    
                    // Angle changed
                    if(i.CurrAngle < i.OgAngle - angleOffSet || i.CurrAngle > i.OgAngle + angleOffSet) 
                    {
                        _logger.LogInformation($"Sign {i.Id} in project {i.ProjectId} is angled incorrectly");
                        await Task.Run(() => {
                            return new SignalRMessageAction("SignAngleIssue", new object[] {i.Id,i.ProjectId,i.CurrAngle});
                        });
                    } 
                }
            }
            return null;
        }

    } // Class end
} // Namespace end
