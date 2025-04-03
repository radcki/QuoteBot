namespace QuotBot.Api.Features.BookQuote.Dto;

public class BookQuoteDto
{
    public int BookQuoteId { get; init; }
    public required string BookTitle { get; init; }
    public required string Author { get; init; }
    public required string Content { get; init; }
    public DateTime CreationDate { get; init; }
}