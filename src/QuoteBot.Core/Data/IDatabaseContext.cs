using Microsoft.EntityFrameworkCore;
using QuotBot.Core.Model;

namespace QuotBot.Core.Data
{
    public interface IDatabaseContext
    {
        DbSet<BookQuote> BookQuotes { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}