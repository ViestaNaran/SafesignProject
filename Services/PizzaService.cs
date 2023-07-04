using SafesignProject.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using SafesignProject.Data;
//using System.Linq;


namespace SafesignProject.Services;

public class PizzaService
{

    //private readonly CosmosConnection _cosmosConnection;
    private readonly Container _container;
    
    public PizzaService(CosmosConnection connection)
    {
      //  _cosmosConnection = connection;
        var client = new CosmosClient(connection.EndpointUri, connection.PrimaryKey);
        var db = client.GetDatabase(connection.DatabaseName);
        _container = db.GetContainer(connection.ContainerName);
    }

    static List<Pizza> Pizzas { get; }
    static int nextId = 3;
    static PizzaService()
    {
        Pizzas = new List<Pizza>
        {
            new Pizza { Id = "1", Name = "Classic Italian", IsGlutenFree = false },
            new Pizza { Id = "2", Name = "Veggie", IsGlutenFree = true }
        };
    }

    public async Task<List<Pizza>> GetAll() {
        var pizzas = _container.GetItemLinqQueryable<Pizza>(true)
        .Select(x => x).ToList<Pizza>();
        
        //var pizIt = pizzas.ToList<Pizza>();
        
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
        // Guid gu = new Guid();
        // pizza.Id = gu;// nextId++.ToString();
        return await _container.CreateItemAsync<Pizza>(pizza);       
        //Pizzas.Add(pizza);
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