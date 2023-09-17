using System.IO.Compression;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Azure.Cosmos;
using Safesign.Core;
using Safesign.Data;

namespace Safesign.Services
{
   public class SignService
   {
        private readonly Container _signContainer;
        private readonly Container _testModelContainer;
        private readonly int signAngleOffSet = 5;
        private readonly int signPositionOffSet = 40;


        public SignService(CosmosConnection connection)
        {
            var client = new CosmosClient(connection.EndpointUri, connection.PrimaryKey);
            var db = client.GetDatabase(connection.SafesignDB);
            var testDB = client.GetDatabase(connection.TestDB);
            _signContainer = db.GetContainer(connection.SignContainer);
            _testModelContainer = testDB.GetContainer(connection.SensorContainer);
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

        public async Task<Sign> GetSignBySensorMac(string sensorDmac)
        {
            var sign = _signContainer.GetItemLinqQueryable<Sign>(true)
            .Where(p => p.SensorId == sensorDmac)
            .AsEnumerable()
            .FirstOrDefault();

            return sign;
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

            if(sign.CurrAngle >= sign.OgAngle-signAngleOffSet && sign.CurrAngle <= sign.OgAngle+signAngleOffSet) {
                return true;
            } else {
                return false;
            }
        }

        private async Task<bool> CheckSignX(Sign sign) {
            
            if(sign.CurrX >= sign.OgX-signPositionOffSet && sign.CurrX <= sign.OgX+signPositionOffSet) {
                return true;
            } else {
                return false;
            }
        }
        
        private async Task<bool> CheckSignY(Sign sign) {
            
            if(sign.CurrY >= sign.OgY-signPositionOffSet && sign.CurrY <= sign.OgY+signPositionOffSet) {
                return true;
            } else {
                return false;
            }
        }

        private async Task<bool> CheckSignZ(Sign sign) {
           
            if(sign.CurrZ >= sign.OgZ-signPositionOffSet && sign.CurrZ <= sign.OgZ+signPositionOffSet) {
                return true;
            } else {
                return false;
            }
        }

        public async Task<(bool?, bool?, bool?)> CheckSignPosition(string signId)
        {
            var sign = await Get(signId);

            (bool? x, bool? y, bool? z) n = (x: null, y: null, z: null);
            
            // n stands for null here.
            if(sign == null) {
                return (n);
            }

            n.x = await CheckSignX(sign);
            n.y = await CheckSignY(sign);
            n.z = await CheckSignZ(sign);

            return n;
        }

        public async Task<Sign> UpdateSensorId(string signId, string newSensorId)
        {
            // 1. Retrieve the existing Sign object by its ID from your data store.
            var existingSign = await Get(signId);

            if (existingSign == null)
            {
                // Handle the case where the Sign with the specified ID doesn't exist.
                return null;
            }

            // 2. Update the "SensorId" property with the new value.
            existingSign.SensorId = newSensorId;

            // 3. Save the updated Sign object back to your data store.
            var updatedSign = await Update(existingSign.Id, existingSign);

            return updatedSign;
        }

        public async Task<Sign> CreateSignWithSensor(string id, string csId, string planId, string macId) 
        {
            // Random random = new Random();
            // string id = random.Next().ToString();
            
            var initialSensorData =  _testModelContainer.GetItemLinqQueryable<TestModel>(true)
                .Where(p => p.dmac == macId)
                .AsEnumerable()
                .FirstOrDefault();

            if(initialSensorData == null) {
                return null;
            }

            // OgAngle not set, because we use it as test value, and sensor does not provide an angle.
            Sign s = new Sign(id, csId, planId, macId)
            {
                OgX = initialSensorData.x0,
                OgY = initialSensorData.y0,
                OgZ = initialSensorData.z0,
                Issue = "None"
            };

            return await _signContainer.CreateItemAsync<Sign>(s);
        } 
   }
}
