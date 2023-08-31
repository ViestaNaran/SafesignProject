using Microsoft.Azure.Cosmos;
using Safesign.Core;
using Safesign.Data;
using System;
using System.Security.Cryptography;
using Safesign.Services;

namespace Safesign.Services
{
    public class ConstructionSiteService
    {
        private readonly Container _constructionSitesContainer;
        private readonly SignService _signService;
       

        public ConstructionSiteService(CosmosConnection connection, SignService signService)
        {
            var client = new CosmosClient(connection.EndpointUri, connection.PrimaryKey);
            var db = client.GetDatabase(connection.SafesignDB);
            _constructionSitesContainer = db.GetContainer(connection.ConstructionSiteContainer);
            _signService = signService;
        }

        public async Task<List<ConstructionSite>> GetAll()
        {
            var csSites= _constructionSitesContainer.GetItemLinqQueryable<ConstructionSite>(true)
            .Select(x => x).ToList<ConstructionSite>();

            return csSites;
        }

        public async Task<ConstructionSite> Get(string id) {
            var csSite = _constructionSitesContainer.GetItemLinqQueryable<ConstructionSite>(true)
            .Where(cs => cs.Id == id)
            .AsEnumerable()
            .FirstOrDefault();

            return csSite;
        }

        public async Task<ConstructionSite> CreateCSsite(ConstructionSite csSite) {
            return await _constructionSitesContainer.CreateItemAsync<ConstructionSite>(csSite);
        }

        public async Task<ConstructionSite> Delete(string id) {
            var csSite = await Get(id);

            if(csSite == null){
                return null;
            }

            return await _constructionSitesContainer.
            DeleteItemAsync<ConstructionSite>(id, new PartitionKey(id));
        }

        public async Task<ConstructionSite> Update(string id, ConstructionSite csSite)
       {
           if (id != csSite.Id)
           {
               return null;
           }
           return await _constructionSitesContainer.ReplaceItemAsync<ConstructionSite>(csSite, id, new PartitionKey(id));
       }

        public async Task<ConstructionSite> CreateCSSiteWithSigns(ConstructionSite csSite, List<Sign> signs)
        {
            // if(signs.Count == 0) {
                
            // }
            foreach(Sign s in signs) {
                await _signService.Add(s);
            }

            return await CreateCSsite(csSite);
        }
    }
}

