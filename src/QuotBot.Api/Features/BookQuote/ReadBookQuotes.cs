using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using QuotBot.Api.Features.BookQuote.Dto;
using QuotBot.Core.Data;

namespace QuotBot.Api.Features.BookQuote
{
    public abstract class ReadBookQuotes
    {


        public class Endpoint(IDatabaseContext databaseContext) : EndpointWithoutRequest<IEnumerable<BookQuoteDto>>
        {
            public override void Configure()
            {
                Get("/api/quote/");
                AllowAnonymous();
            }

            public override async Task HandleAsync(CancellationToken ct)
            {
                var data = databaseContext.BookQuotes
                                          .OrderByDescending(x => x.CreationDate)
                                          .Select(x => new BookQuoteDto()
                                                       {
                                                           Content = x.Content,
                                                           Author = x.Author,
                                                           BookTitle = x.BookTitle,
                                                           CreationDate = x.CreationDate,
                                                           BookQuoteId = x.BookQuoteId
                                                       });

                await SendAsync(data, cancellation: ct);
            }
        }
    }
}