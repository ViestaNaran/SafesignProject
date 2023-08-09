//using Microsoft.Azure.Cosmos;
//using Safesign.Core;
//using Safesign.Data;
//using System.ComponentModel;

//namespace Safesign.Api.Services
//{
//    public class PlanService
//    {
//        private readonly Microsoft.Azure.Cosmos.Container _planContainer;

//        public PlanService(CosmosConnection connection)
//        {
//            var client = new CosmosClient(connection.EndpointUri, connection.PrimaryKey);
//            var db = client.GetDatabase(connection.SafesignDB);
//            _planContainer = db.GetContainer(connection.PlanContainer);
//        }

//        public async Task<List<Plan>> GetAll()
//        {
//            var pizzas = _planContainer.GetItemLinqQueryable<Pizza>(true)
//            .Select(x => x).ToList<Pizza>();

//            return pizzas;
//        }

//        public async Task<Pizza> Get(string id)
//        {
//            var pizza = _planContainer.GetItemLinqQueryable<Pizza>(true)
//            .Where(p => p.Id == id)
//            .AsEnumerable()
//            .FirstOrDefault();

//            return pizza;
//        }

//        public async Task<Pizza> Add(Pizza pizza)
//        {
//            return await _planContainer.CreateItemAsync<Pizza>(pizza);
//        }

//        public async Task<Pizza> Delete(string id)
//        {
//            var pizza = Get(id);
//            if (pizza is null)
//                return null;

//            return await _planContainer.DeleteItemAsync<Pizza>(id, new PartitionKey(id));
//        }

//        public async Task<Pizza> Update(string id, Pizza pizza)
//        {
//            if (id != pizza.Id)
//            {
//                return null;
//            }
//            return await _planContainer.ReplaceItemAsync<Pizza>(pizza, id, new PartitionKey(id));
//        }

//    }
//}
