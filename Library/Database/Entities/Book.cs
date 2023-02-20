using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("Book")]
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Cover { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string Genere { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}