using FastEndpoints;
using QuotBot.Core.Data;

namespace QuotBot.Api.Features.BookQuote
{
    public abstract class DeleteAllBookQuotes
    {
        public class Endpoint(IDatabaseContext databaseContext) : EndpointWithoutRequest
        {
            public override void Configure()
            {
                Delete("/api/quote/all");
                AllowAnonymous();
            }

            public override async Task HandleAsync(CancellationToken ct)
            {
                foreach (var bookQuote in databaseContext.BookQuotes.ToList())
                {
                    databaseContext.BookQuotes.Remove(bookQuote);
                }

                await databaseContext.SaveChangesAsync(ct);

                await SendNoContentAsync(ct);
            }
        }
    }
}