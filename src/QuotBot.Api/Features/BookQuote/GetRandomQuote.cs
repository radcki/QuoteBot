using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using QuotBot.Api.Features.BookQuote.Dto;
using QuotBot.Core.Data;

namespace QuotBot.Api.Features.BookQuote
{
    public abstract class GetRandomQuote
    {
        public class Endpoint(IDatabaseContext databaseContext) : EndpointWithoutRequest<BookQuoteDto?>
        {
            public override void Configure()
            {
                Get("/api/quote/random");
                AllowAnonymous();
            }

            public override async Task HandleAsync(CancellationToken ct)
            {
                int total = databaseContext.BookQuotes.Count();
                Random random = new Random();
                int offset = random.Next(0, total);
                var result = await databaseContext.BookQuotes.Select(x => new BookQuoteDto()
                                                                          {
                                                                              Content = x.Content,
                                                                              Author = x.Author,
                                                                              BookTitle = x.BookTitle,
                                                                              CreationDate = x.CreationDate,
                                                                              BookQuoteId = x.BookQuoteId
                                                                          })
                                                  .Skip(offset)
                                                  .FirstOrDefaultAsync(cancellationToken: ct);

                await SendAsync(result, cancellation: ct);
            }
        }
    }
}