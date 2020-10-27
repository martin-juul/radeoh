using System;
using Newtonsoft.Json;
using Xamarin.Forms;

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
        public string SecureImageUrl => Image.Replace("http", "https");
        
        public UriImageSource CachedImageSource => new UriImageSource
        {
            Uri = new Uri(SecureImageUrl),
            CachingEnabled = true,
            CacheValidity = new TimeSpan(1, 0, 0, 0, 0)
        };

        [JsonProperty("subtext")]
        public string Subtext { get; set; } 

        [JsonProperty("bitrate")]
        public string Bitrate { get; set; } 

        [JsonProperty("stream_url")]
        public string StreamUrl { get; set; }

        public string InsecureStreamUrl => StreamUrl.Replace("https", "http");
    }
}