using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuotBot.Core.Services.KindleClippingsLoader;
using Shouldly;

namespace QuoteBot.Core.UnitTests
{
    public class KindleClippingsFileQuoteLoaderTests
    {
        [Fact]
        public async Task LoadClippings_Shhould_ReturnAllEntries()
        {
            // Arrange
            var loader = new KindleClippingsFileQuoteLoader(new KindleClippingQuoteParser());

            // Act
            var results = await loader.LoadQuotes(@"Data/tast-data.txt");
            results?.Count.ShouldBe(111);
        }
    }
}