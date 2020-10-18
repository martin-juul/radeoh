using Newtonsoft.Json;

namespace Radeoh.Models
{
    public class Station
    {
        [JsonProperty("title")]
        public string Title { get; set; } 

        [JsonProperty("slug")]
        public string Slug { get; set; } 

        [JsonProperty("country")]
        public string Country { get; set; } 

        [JsonProperty("lang")]
        public string Lang { get; set; } 

        [JsonProperty("image")]
        public string Image { get; set; } 

        [JsonProperty("subtext")]
        public object Subtext { get; set; } 

        [JsonProperty("bitrate")]
        public object Bitrate { get; set; } 

        [JsonProperty("stream_url")]
        public object StreamUrl { get; set; } 
    }
}