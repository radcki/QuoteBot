using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuoteBot.Core.Model;
using QuoteBot.Core.Services.ClippingsLoader;

namespace QuoteBot.Core.UnitTests
{
    public class FileClippingsLoaderTests
    {
        [Fact]
        public async Task A()
        {
            // Arrange
            var loader = new FileClippingsLoader(new FileClippingsLoaderConfiguration()
                                                 {
                                                     FilePath = @"Data/tast-data.txt"
                                                 }, new ClippingsParser());

            // Act
            var results = new List<Clipping>();
            await foreach (var cliping in loader.LoadClippings())
            {
                results.Add(cliping);
            }
        }
    }
}