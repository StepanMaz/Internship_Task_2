using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("Review")]
    public class Review
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Reviewer { get; set; }

        public int BookID { get; set; }
        public Book Book { get; set; }
    }
}