namespace DTO
{
    public class ExpandedBookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Rating { get; set; }
        public string Cover { get; set; }
        public string Content { get; set; }
        public IEnumerable<ExpandedBookDTOReviewDTO> Reviews { get; set; }

        public class ExpandedBookDTOReviewDTO
        {
            public int Id { get; set; }
            public string Message { get; set; }
            public string Reviewer { get; set; }
        }
    }
}