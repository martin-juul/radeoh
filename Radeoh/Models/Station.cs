using SQLite;

namespace Radeoh.Models
{
    public class Station
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        [Unique]
        public string Slug { get; set; }
        public string Country { get; set; }
        public string Lang { get; set; }
        public string Subtext { get; set; }
        public string Bitrate { get; set; }
        public string StreamUrl { get; set; }
    }
}