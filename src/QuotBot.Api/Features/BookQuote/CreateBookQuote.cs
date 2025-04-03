using FastEndpoints;
using QuotBot.Core.Data;

namespace QuotBot.Api.Features.BookQuote
{
    public abstract class CreateBookQuote
    {
        public class Request
        {
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
                Post("/api/quote/");
                AllowAnonymous();
            }

            public override async Task HandleAsync(Request req, CancellationToken ct)
            {
                var entity = new Core.Model.BookQuote
                             {
                                 Author = req.Author,
                                 Content = req.Content,
                                 BookTitle = req.BookTitle
                             };

                databaseContext.BookQuotes.Add(entity);
                await databaseContext.SaveChangesAsync(ct);

                await SendAsync(new()
                                {
                                }, cancellation: ct);
            }
        }
    }
}