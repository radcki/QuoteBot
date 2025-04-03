using FastEndpoints;
using QuotBot.Core.Data;

namespace QuotBot.Api.Features.BookQuote
{
    public abstract class UpdateBookQuote
    {
        public class Request
        {
            public int BookQuoteId { get; set; }
            public required string Author { get; init; }
            public required string BookTitle { get; init; }
            public required string Content { get; init; }
        }

        public class Result
        {
        }

        public class Endpoint(IDatabaseContext databaseContext) : Endpoint<Request, Result>
        {
            public override void Configure()
            {
                Put("/api/quote/{BookQuoteId:int}");
                AllowAnonymous();
            }

            public override async Task HandleAsync(Request req, CancellationToken ct)
            {
                var entity = databaseContext.BookQuotes.FirstOrDefault(x => x.BookQuoteId == req.BookQuoteId);
                if (entity == null)
                {
                    throw new KeyNotFoundException("Book not found");
                }

                entity.Author = req.Author;
                entity.Content = req.Content;
                entity.BookTitle = req.BookTitle;

                await databaseContext.SaveChangesAsync(ct);

                await SendAsync(new()
                                {
                                }, cancellation: ct);
            }
        }
    }
}