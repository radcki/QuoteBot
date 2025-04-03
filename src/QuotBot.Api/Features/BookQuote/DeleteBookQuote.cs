using FastEndpoints;
using QuotBot.Core.Data;

namespace QuotBot.Api.Features.BookQuote
{
    public abstract class DeleteBookQuote
    {
        public class Request
        {
            public int BookQuoteId { get; init; }
        }


        public class Endpoint(IDatabaseContext databaseContext) : Endpoint<Request>
        {
            public override void Configure()
            {
                Delete("/api/quote/{BookQuoteId:int}");
                AllowAnonymous();
            }

            public override async Task HandleAsync(Request req, CancellationToken ct)
            {
                var entity = databaseContext.BookQuotes.FirstOrDefault(x => x.BookQuoteId == req.BookQuoteId);
                if (entity == null)
                {
                    throw new KeyNotFoundException("Book not found");
                }

                databaseContext.BookQuotes.Remove(entity);
                await databaseContext.SaveChangesAsync(ct);

                await SendNoContentAsync(ct);
            }
        }
    }
}