using Microsoft.Azure.Cosmos;
using Safesign.Core;
using Safesign.Data;
using System;

namespace Safesign.Services
{
   public class PlanService
   {
       private readonly Container _planContainer;
       private readonly SignService _signService;
       private readonly Random random;

       public PlanService(CosmosConnection connection, SignService signService)
       {
           var client = new CosmosClient(connection.EndpointUri, connection.PrimaryKey);
           var db = client.GetDatabase(connection.SafesignDB);
           _planContainer = db.GetContainer(connection.PlanContainer);
           _signService = signService;
           random = new Random();
       }

       public async Task<List<Plan>> GetAll()
       {
           var plans = _planContainer.GetItemLinqQueryable<Plan>(true)
           .Select(x => x).ToList<Plan>();

           return plans;
       }

       public async Task<Plan> Get(string id)
        {
           var plan = _planContainer.GetItemLinqQueryable<Plan>(true)
           .Where(p => p.Id == id)
           .AsEnumerable()
           .FirstOrDefault();

        
           return plan;
        }

        public async Task<List<Sign>> GetPlanSigns(string planId) 
        {
            return await _signService.GetSignsByPlanId(planId);
        }

       public async Task<Plan> Add(Plan plan)
        {
           // plan.Signs = new List<Sign>();
            return await _planContainer.CreateItemAsync<Plan>(plan);
        }

       public async Task<Plan> Delete(string id)
       {
           var plan = await Get(id);
           if (plan is null)
               return null;

            var planSigns = await _signService.GetSignsByPlanId(plan.Id);
            
            foreach(Sign s in planSigns) 
            {
                await _signService.Delete(s.Id);
            }

           return await _planContainer.DeleteItemAsync<Plan>(id, new PartitionKey(id));
       }

       public async Task<Plan> Update(string id, Plan plan)
       {
           if (id != plan.Id)
           {
               return null;
           }
           return await _planContainer.ReplaceItemAsync<Plan>(plan, id, new PartitionKey(id));
       }
   }
}
