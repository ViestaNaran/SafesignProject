namespace Safesign.Data {
    public class CosmosConnection {
        public string? EndpointUri {get; set;}
        public string? PrimaryKey {get; set;}
        public string? PizzaDB {get; set;}
        public string? SafesignDB { get; set; }
        public string? ContainerName {get; set;}
    }
}