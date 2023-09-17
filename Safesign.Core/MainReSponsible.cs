using Newtonsoft.Json;

namespace Safesign.Core
{
    public class MainReSponsible
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}