using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Safesign.Core;

namespace Safesign.AzureFunction
{
    public class DetectNoGluten
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        
        public DetectNoGluten(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory)
        {
            _logger = loggerFactory.CreateLogger<DetectNoGluten>();
            _httpClient = httpClientFactory.CreateClient("Api");
        }

        [Function("DetectNoGluten")]
        public async Task RunAsync([CosmosDBTrigger(
            databaseName: "PizzaDatabase",
            collectionName: "PizzaContainer",
            ConnectionStringSetting = "CosmosConnectionString",CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "leases")] IReadOnlyList<Pizza> input)
        {
            if (input != null && input.Count > 0)
            {
                _logger.LogInformation("Documents modified: " + input.Count);
                _logger.LogInformation("First document Id: " + input[0].Id);
                foreach(var i in input)
                {
                    _logger.LogInformation("Documents modified: " + i.Id);
                    _logger.LogInformation("IsGlutenFree: " + i.IsGlutenFree);

                    if(!i.IsGlutenFree)
                    {
                        _logger.LogInformation("Test");
                        var respone = await _httpClient.GetAsync($"Pizza/{i.Id}");
                        _logger.LogInformation(await respone.Content.ReadAsStringAsync());
                    }
                }
            }
        }
    }
}
