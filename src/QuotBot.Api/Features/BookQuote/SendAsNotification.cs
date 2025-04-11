using FastEndpoints;
using QuotBot.Core.Data;
using QuotBot.Core.Services.PushoverPublisher;

namespace QuotBot.Api.Features.BookQuote
{
    public abstract class SendAsNotification
    {
        public class Request
        {
            public int BookQuoteId { get; init; }
        }


        public class Endpoint(IDatabaseContext databaseContext, PushoverPublisherService pushoverPublisherService) : Endpoint<Request>
        {
            public override void Configure()
            {
                Post("/api/quote/{BookQuoteId:int}/send-as-notification/");
                AllowAnonymous();
            }

            public override async Task HandleAsync(Request req, CancellationToken ct)
            {
                var entity = databaseContext.BookQuotes.FirstOrDefault(x => x.BookQuoteId == req.BookQuoteId);
                if (entity == null)
                {
                    throw new KeyNotFoundException("Book not found");
                }

                await pushoverPublisherService.PublishNotification($"\"{entity.Content}\"", $"{entity.BookTitle} - {entity.Author}");

                await SendNoContentAsync(ct);
            }
        }
    }
}