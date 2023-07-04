using Newtonsoft.Json;

namespace SafesignProject.Models;

public class Pizza
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public string? Name { get; set; }
    public bool IsGlutenFree { get; set; }
}