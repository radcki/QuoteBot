using FastEndpoints;
using QuotBot.Core.Data;
using QuotBot.Core.Services.KindleClippingsLoader;
using System.Text;
using Microsoft.EntityFrameworkCore;
using QuotBot.Api.Extensions;

namespace QuotBot.Api.Features.BookQuote
{
    public abstract class ImportKindleClippings
    {
        public class Result
        {
            public int AddedCount { get; init; }
        }

        public class Endpoint(IDatabaseContext databaseContext, KindleClippingQuoteParser kindleClippingQuoteParser, ILogger<ImportKindleClippings> logger) : EndpointWithoutRequest<Result>
        {
            public override void Configure()
            {
                Post("/api/quote/import-kindle-clippings");

                AllowFileUploads();
                AllowAnonymous();
            }


            public override async Task HandleAsync(CancellationToken ct)
            {
                var addedCount = 0;
                var existingQuotes = await databaseContext.BookQuotes.ToListAsync(cancellationToken: ct);
                logger.LogInformation($"Uploaded files count: {Files.Count}");
                foreach (var file in Files)
                {
                    var input = await file.ReadAsStringAsync(ct);

                    foreach (var bookQuote in kindleClippingQuoteParser.ParseQuotes(input))
                    {
                        var quoteAdded = existingQuotes.Any(x => x.Content == bookQuote.Content && x.BookTitle == bookQuote.BookTitle);
                        if (quoteAdded)
                        {
                            continue;
                        }

                        databaseContext.BookQuotes.Add(bookQuote);
                        addedCount++;
                    }

                    if (addedCount > 0)
                    {
                        await databaseContext.SaveChangesAsync(ct);
                    }
                }

                await SendAsync(new()
                                {
                                    AddedCount = addedCount
                                }, cancellation: ct);
            }
        }
    }
}