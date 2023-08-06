using Safesign.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Safesign.Data;
//using System.Linq;


namespace Safesign.Services;

public class PizzaService
{

    //private readonly CosmosConnection _cosmosConnection;
    private readonly Container _container;
    
    public PizzaService(CosmosConnection connection)
    {
        var client = new CosmosClient(connection.EndpointUri, connection.PrimaryKey);
        var db = client.GetDatabase(connection.PizzaDB);
        _container = db.GetContainer(connection.ContainerName);
    }

    public async Task<List<Pizza>> GetAll() {
        var pizzas = _container.GetItemLinqQueryable<Pizza>(true)
        .Select(x => x).ToList<Pizza>();
        
        return pizzas;
    }

    public async Task<Pizza> Get(string id) 
    {
        var pizza = _container.GetItemLinqQueryable<Pizza>(true)
        .Where(p => p.Id == id)
        .AsEnumerable()
        .FirstOrDefault();

        return pizza;
    } 

    public async Task<Pizza> Add(Pizza pizza)
    {
        return await _container.CreateItemAsync<Pizza>(pizza);       
    }

    public async Task<Pizza> Delete(string id)
    {
        var pizza = Get(id);
        if(pizza is null)
            return null;

       return await _container.DeleteItemAsync<Pizza>(id, new PartitionKey(id));
    }

    public async Task<Pizza> Update(string id, Pizza pizza)
    {
        if(id != pizza.Id) {
            return null;
        }            
        return await _container.ReplaceItemAsync<Pizza>(pizza,id, new PartitionKey(id));
    }
}