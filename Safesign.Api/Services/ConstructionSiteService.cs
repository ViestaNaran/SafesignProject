using Microsoft.Azure.Cosmos;
using Safesign.Core;
using Safesign.Data;
using System;

namespace Safesign.Services
{
    public class ConstructionSiteService
    {
        private readonly Container _constructionSitesContainer;
       

        public ConstructionSiteService(CosmosConnection connection)
        {
            var client = new CosmosClient(connection.EndpointUri, connection.PrimaryKey);
            var db = client.GetDatabase(connection.SafesignDB);
            _constructionSitesContainer = db.GetContainer(connection.ConstructionSiteContainer);
        }

        public async Task<List<ConstructionSite>> GetAll()
        {
            var csSites= _constructionSitesContainer.GetItemLinqQueryable<ConstructionSite>(true)
            .Select(x => x).ToList<ConstructionSite>();

            return csSites;
        }
    }
}