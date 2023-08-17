using System;
using Safesign.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Safesign.AzureFunction
{
    public class DetectSignChanges
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly int angleOffSet = 5;

        public DetectSignChanges(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory) 
        {
            _logger = loggerFactory.CreateLogger<DetectSignChanges>();
            _httpClient = httpClientFactory.CreateClient("Api");
        }


        [Function("DetectSignChanges")]
        public async Task RunAsync([CosmosDBTrigger(
            databaseName: "Safesign",
            collectionName: "Signs",
            ConnectionStringSetting = "CosmosConnectionString", CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "SignLeases")] IReadOnlyList<Sign> input) 
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
                        
                    }
                }
            }
        }
    }
}