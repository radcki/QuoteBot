namespace QuotBot.Core.Model
{
    public class BookQuote
    {
        public int BookQuoteId { get; set; }
        public required string BookTitle { get; set; }
        public required string Author { get; set; }
        public required string Content { get; set; }
        public DateTime CreationDate { get; set; }
    }
}