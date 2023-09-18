using Microsoft.Azure.Cosmos;
using Safesign.Core;
using Safesign.Data;

namespace Safesign.Services
{
    public class ConstructionSiteService
    {
        private readonly Container _constructionSitesContainer;
        private readonly PlanService _planService;
        private readonly SignService _signService;

        public ConstructionSiteService(CosmosConnection connection, PlanService planService, SignService signService)
        {
            var client = new CosmosClient(connection.EndpointUri, connection.PrimaryKey);
            var db = client.GetDatabase(connection.SafesignDB);
            _constructionSitesContainer = db.GetContainer(connection.ConstructionSiteContainer);
            _planService = planService;
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

        public async Task<ConstructionSite> CreateCsite(ConstructionSite csSite) {
            return await _constructionSitesContainer.CreateItemAsync<ConstructionSite>(csSite);
        }

        public async Task<ConstructionSite> CreateCSsite1(ConstructionSite csSite)
        {
        // Generate a unique ID for the ConstructionSite
            csSite.Id = Guid.NewGuid().ToString();

        // Create the ConstructionSite in the database
            return await _constructionSitesContainer.CreateItemAsync<ConstructionSite>(csSite);
        }

        public async Task<ConstructionSite> Delete(string id) {
            var csSite = await Get(id);

            if(csSite == null){
                return null;
            }

            if(csSite.PlanId != null)
            {
                await _planService.Delete(csSite.PlanId);
            }
            
            var signs = await _signService.GetSignsByCSId(id);
            
            foreach(Sign s in signs) 
            {
                if(s == null) {  
                    continue;
                }
                await _signService.Delete(s.Id);
            }
            return await _constructionSitesContainer.DeleteItemAsync<ConstructionSite>(id, new PartitionKey(id));
        }

        public async Task<ConstructionSite> Update(string id, ConstructionSite csSite)
       {
           if (id != csSite.Id)
           {
               return null;
           }
           return await _constructionSitesContainer.ReplaceItemAsync<ConstructionSite>(csSite, id, new PartitionKey(id));
       }

        public async Task<ConstructionSite> CreateCsiteWithSigns(ConstructionSite csSite, List<Sign> signs)
        {
            foreach(Sign s in signs) {
                await _signService.Add(s);
            }

            return await CreateCsite(csSite);
        }

        public async Task<(string signId, ConstructionSite csSite)> CreateCsiteWithSignMacId(ConstructionSite csSite, List<Sign> signs)
        {
            foreach (Sign s in signs)
            {
                var result = await _signService.CreateSignWithSensor(s.Id, s.CSId, s.PlanId, s.SensorId);

                if (result == null)
                {
                    // Return the ID of the sign and null value
                    return (s.Id, null);
                }
            }

            return (null, await CreateCsite(csSite));
        }
    }
}

