using QuotBot.Core.Model;

namespace QuotBot.Core.Services.KindleClippingsLoader;

public class KindleClippingQuoteParser
{
    public IEnumerable<BookQuote> ParseQuotes(string input)
    {
        var entries = SplitClippings(input);
        foreach (var clipping in entries)
        {
            var bookQuote = ParseClipping(clipping);
            if (bookQuote != null)
                yield return bookQuote;
        }
    }

    private IEnumerable<string> SplitClippings(string input)
    {
        return input.Split("==========", StringSplitOptions.TrimEntries);
    }

    private BookQuote? ParseClipping(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.TrimEntries);
        if (lines.Length < 4)
            return null;

        var titleLine = lines[0];
        var metaLine = lines[1];
        var content = string.Join(Environment.NewLine, lines[3..]);

        var author = ParseAuthorFromTitleLine(titleLine);
        var title = ParseTitleFromTitleLine(titleLine, author) ?? string.Empty;

        return new BookQuote() { BookTitle = title, Author = author, Content = content, CreationDate = DateTime.MinValue };
    }

    private string? ParseAuthorFromTitleLine(string input)
    {
        var parts = SplitFirstParenthesesLevel(input);
        return parts.LastOrDefault();
    }

    private string? ParseTitleFromTitleLine(string input, string author)
    {
        var inputWithoutAuthor = input.Replace(author, string.Empty)
                                      .Replace("()", string.Empty)
                                      .TrimEnd(' ', '-');

        return inputWithoutAuthor;
    }

    private List<string> SplitFirstParenthesesLevel(string input)
    {
        var result = new List<string>();
        int p = 0, level = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == '(')
            {
                level++;
                if (level == 1)
                    p = i + 1;
            }
            else if (input[i] == ')')
                if (--level == 0)
                    result.Add(input.Substring(p, i - p));
        }

        return result;
    }
}