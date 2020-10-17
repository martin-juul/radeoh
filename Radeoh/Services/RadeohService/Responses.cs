namespace Radeoh.Services.RadeohService
{
    public class Responses
    {
        public class Station
        {
            public string Title { get; set; }
            public string Slug { get; set; }
            public string Country { get; set; }
            public string Lang { get; set; }
            public string Subtext { get; set; }
            public string Bitrate { get; set; }
            public string StreamUrl { get; set; }
        }
    }
}