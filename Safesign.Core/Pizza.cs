using Newtonsoft.Json;

namespace Safesign.Core;

public class Pizza
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public string? Name { get; set; }
    public bool IsGlutenFree { get; set; }
}