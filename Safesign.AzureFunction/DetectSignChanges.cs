using Safesign.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.IO.Compression;

namespace Safesign.AzureFunction
{
    public class DetectSignChanges
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly int angleOffSet = 5;
        private readonly int positionOffSet = 40;
    

        [Function("negotiate")]
        public static HttpResponseData Negotiate([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequestData req,
        [SignalRConnectionInfoInput(HubName = "serverless")] string connectionInfo)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            response.WriteString(connectionInfo);
            return response;
        }
        
        [Function("index")]
        public static HttpResponseData GetHomePage([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteString(File.ReadAllText("content/index.html"));
            response.Headers.Add("Content-Type", "text/html");
            return response;
        }


        public DetectSignChanges(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory) 
        {
            _logger = loggerFactory.CreateLogger<DetectSignChanges>();
            _httpClient = httpClientFactory.CreateClient("Api");
        }


        [Function(nameof(DetectAngleChange2))]
        [SignalROutput(HubName = "serverless")]
        public SignalRMessageAction DetectAngleChange2(
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
                        _logger.LogInformation($"Sign {i.Id} in project {i.PlanId} is angled incorrectly");
                       // await Task.Run(() => {
                        return new SignalRMessageAction("SignAngleIssue", new object[] {i});
                       // });
                    }
                }
            }
            return null;
        }

        [Function(nameof(DetectPositionChanges))]
        [SignalROutput(HubName = "serverless")]
        public SignalRMessageAction DetectPositionChanges(
            [CosmosDBTrigger(
            databaseName: "Safesign",
            collectionName: "SensorTest",
            ConnectionStringSetting = "CosmosConnectionString", CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "SignLeases")] IReadOnlyList<Sign> input
            )
        {
            _logger.LogInformation("DetectSensorchanges running!");

            if(input != null && input.Count > 0) 
            {
                _logger.LogInformation($"Documents Modified: {input.Count}");
                
                foreach(var i in input) 
                {
                    Console.WriteLine(i);
                    _logger.LogInformation($"i = {i}, {i.OgZ}");
                    _logger.LogInformation($"Sensor Modified: {i.SensorId}, on Sign: {i.Id}");
                  
                    // Z coordinate of sign has changed above / Below threshold. 
                    // if(i.CurrZ <= 0) 
                    // {
                    //     _logger.LogInformation($"Sensor {i.SensorId} has z less than -200");
                    //    // await Task.Run(() => {
                    //     return new SignalRMessageAction("SensorDataIssue", new object[] {i});
                    //    // });
                    // }

                    // X coordinate of sign has changed above / Below threshold. 
                    if(i.CurrX >= i.OgX-positionOffSet && i.CurrX <= i.OgX+positionOffSet)
                    {
                        _logger.LogInformation($"Sensor {i.SensorId} has z less than -200");
                        i.Issue += "X";
                        return new SignalRMessageAction("SignPositionIssue", new object[] {i});
                    }

                    // Y coordinate of sign has changed above / Below threshold. 
                    if(i.CurrY >= i.OgY-positionOffSet && i.CurrY <= i.OgY+positionOffSet)
                    {
                        _logger.LogInformation($"Sensor {i.SensorId}");
                        i.Issue += "Y";
                        return new SignalRMessageAction("SignPositionIssue", new object[] {i});
                    }

                    // Z coordinate of sign has changed above / Below threshold. 
                    if(i.CurrZ >= i.OgZ-positionOffSet && i.CurrZ <= i.OgZ+positionOffSet)
                    {
                        _logger.LogInformation($"Sensor {i.SensorId} has z less than -200");
                        i.Issue += "Z";
                        return new SignalRMessageAction("SignPositionIssue", new object[] {i});
                    }
                }
            }
            return null;
        }
    } // Class end
} // Namespace end
