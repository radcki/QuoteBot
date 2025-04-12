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
        public class Request
        {
            public string? Clippings { get; set; } = null;
        }

        public class Result
        {
            public int AddedCount { get; init; }
        }

        public class Endpoint(IDatabaseContext databaseContext, KindleClippingQuoteParser kindleClippingQuoteParser, ILogger<ImportKindleClippings> logger) : Endpoint<Request, Result>
        {
            public override void Configure()
            {
                Post("/api/quote/import-kindle-clippings");
                
                AllowAnonymous();
            }


            public override async Task HandleAsync(Request req, CancellationToken ct)
            {
                var existingQuotes = await databaseContext.BookQuotes.ToListAsync(cancellationToken: ct);
                List<Core.Model.BookQuote> newBookQuotes = [];
    
                if (!string.IsNullOrWhiteSpace(req.Clippings))
                {
                    newBookQuotes.AddRange(GetNewQuotes(req.Clippings, existingQuotes));
                }

                if (newBookQuotes.Count > 0)
                {
                    databaseContext.BookQuotes.AddRange(newBookQuotes);
                    await databaseContext.SaveChangesAsync(ct);
                }

                await SendAsync(new()
                                {
                                    AddedCount = newBookQuotes.Count
                                }, cancellation: ct);
            }

            private IEnumerable<Core.Model.BookQuote> GetNewQuotes(string input, List<Core.Model.BookQuote> existingQuotes)
            {
                var addedCount = 0;
                foreach (var bookQuote in kindleClippingQuoteParser.ParseQuotes(input))
                {
                    var quoteAdded = existingQuotes.Any(x => x.Content == bookQuote.Content && x.BookTitle == bookQuote.BookTitle);
                    if (quoteAdded)
                    {
                        continue;
                    }

                    yield return bookQuote;

                    databaseContext.BookQuotes.Add(bookQuote);
                }
            }
        }
    }
}