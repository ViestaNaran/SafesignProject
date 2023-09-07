using Microsoft.Azure.Cosmos;
using Safesign.Core;
using Safesign.Data;

namespace Safesign.Services
{
   public class SignService
   {
        private readonly Container _signContainer;

        private readonly int signOffset = 5;
        public SignService(CosmosConnection connection)
        {
            var client = new CosmosClient(connection.EndpointUri, connection.PrimaryKey);
            var db = client.GetDatabase(connection.SafesignDB);
            _signContainer = db.GetContainer(connection.SignContainer);
        }

        public async Task<List<Sign>> GetAll()
        {
            var signs = _signContainer.GetItemLinqQueryable<Sign>(true)
            .Select(x => x).ToList<Sign>();

            return signs;
        }

        public async Task<Sign> Get(string id)
        {
            var sign = _signContainer.GetItemLinqQueryable<Sign>(true)
            .Where(p => p.Id == id)
            .AsEnumerable()
            .FirstOrDefault();

            return sign;
        }

        public async Task<List<Sign>> GetSignsByPlanId(string planId)
        {
            var signs = _signContainer.GetItemLinqQueryable<Sign>(true)
            .Where(p => p.PlanId == planId)
            .AsEnumerable()
            .ToList();

            return signs;
        }

        public async Task<List<Sign>> GetSignsByCSId(string constructionSiteId)
        {
            var signs = _signContainer.GetItemLinqQueryable<Sign>(true)
            .Where(p => p.CSId == constructionSiteId)
            .AsEnumerable()
            .ToList();

            return signs;
        }

        public async Task<Sign> Add(Sign sign)
        {
            return await _signContainer.CreateItemAsync<Sign>(sign);
        }

        // In progress
        public async Task<Sign> Add(string id, string csId, string planId, float angle)
        {
            Sign sign = new Sign(id, csId, planId, angle);
            return await _signContainer.CreateItemAsync<Sign>(sign);
        }

        public async Task<Sign> Delete(string id)
        {
            var sign = Get(id);
            if (sign is null)
                return null;

            return await _signContainer.DeleteItemAsync<Sign>(id, new PartitionKey(id));
        }

        public async Task<Sign> Update(string id, Sign sign)
        {
            if (id != sign.Id)
            {
                return null;
            }
            return await _signContainer.ReplaceItemAsync<Sign>(sign, id, new PartitionKey(id));
        }

        public async Task<bool> CheckSignAngle(string signId) {
            
            var sign = await Get(signId);

            if(sign == null) {
                return false;
            }    

            if(sign.CurrAngle >= sign.OgAngle-signOffset && sign.CurrAngle <= sign.OgAngle+signOffset) {
                return true;
            } else {
                return false;
            }
        }
   }
}
