using QuotBot.Core.Model;

namespace QuotBot.Core.Services.KindleClippingsLoader
{
    public class KindleClippingsFileQuoteLoader(KindleClippingQuoteParser parser)
    {
        public async Task<List<BookQuote>?> LoadQuotes(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var content = await File.ReadAllTextAsync(path);
            return parser.ParseQuotes(content).ToList();
        }
    }
}