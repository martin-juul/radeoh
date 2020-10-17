using Newtonsoft.Json;

namespace Radeoh.Models
{
    public class Station
    {
        [JsonProperty]
        public string Title { get; set; }
        [JsonProperty]
        public string Slug { get; set; }
        [JsonProperty]
        public string Country { get; set; }
        [JsonProperty]
        public string Lang { get; set; }
        [JsonProperty]
        public string Subtext { get; set; }
        [JsonProperty]
        public string Bitrate { get; set; }
        
    }
}