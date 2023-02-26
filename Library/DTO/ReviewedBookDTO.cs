namespace DTO
{
    public class ReviewedBookDTO
    {
        public int Id { get; set; }
        public string Cover {get; set;}
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Rating { get; set; }
        public int ReviewNumber { get; set; }
    }
}