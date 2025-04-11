using Microsoft.EntityFrameworkCore;
using QuotBot.Api.Features.BookQuote.Dto;
using QuotBot.Core.Data;
using QuotBot.Core.Services.PushoverPublisher;

namespace QuotBot.Api.Jobs
{
    public class SendRandomQuoteAsNotificationJob(IDatabaseContext databaseContext, PushoverPublisherService pushoverPublisherService)
    {
        public async Task Execute()
        {
            int total = databaseContext.BookQuotes.Count();
            Random random = new Random();
            int offset = random.Next(0, total);
            var quote = await databaseContext.BookQuotes
                                             .Skip(offset)
                                             .FirstOrDefaultAsync();
            if (quote == null)
                return;

            await pushoverPublisherService.PublishNotification($"\"{quote.Content}\"", $"{quote.BookTitle} - {quote.Author}");
        }
    }
}