using SQLite;

namespace Radeoh.DAL.Models
{
    public class Favorite
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }
        
        [Column("slug"), Unique]
        public string Slug { get; set; }
    }
}