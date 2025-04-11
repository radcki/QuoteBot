using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuotBot.Core.Model;

namespace QuotBot.Core.Data.Configuration
{
    internal class BookQuoteConfiguration : IEntityTypeConfiguration<BookQuote>
    {
        #region Implementation of IEntityTypeConfiguration<BookQuote>

        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<BookQuote> builder)
        {
            builder.HasKey(x => x.BookQuoteId);
            builder.Property(x => x.BookQuoteId).ValueGeneratedOnAdd();
        }

        #endregion
    }
}
