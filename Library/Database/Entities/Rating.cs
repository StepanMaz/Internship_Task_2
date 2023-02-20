using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("Rating")]
    public class Rating
    {
        public int Id { get; set;}

        public int BookID { get; set;}
        public Book Book { get; set; }
        
        public int Score { get; set; }
    }
}