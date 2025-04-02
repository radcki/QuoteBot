using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using QuoteBot.Core.Model;

namespace QuoteBot.Core.Services.ClippingsLoader
{
    public class FileClippingsLoaderConfiguration
    {
        public required string FilePath { get; init; }
    }

    public class FileClippingsLoader(FileClippingsLoaderConfiguration configuration, ClippingsParser parser)
    {
        private readonly FileClippingsLoaderConfiguration _configuration = configuration;
        private readonly ClippingsParser _parser = parser;

        public async IAsyncEnumerable<Clipping> LoadClippings()
        {
            var path = configuration.FilePath;
            if (!File.Exists(path))
            {
                yield break;
            }

            var content = await File.ReadAllTextAsync(path);
            foreach (var clipping in _parser.ParseClippings(content))
            {
                yield return clipping;
            }
        }
    }

    public class ClippingsParser
    {
        public IEnumerable<Clipping> ParseClippings(string input)
        {
            var entries = SplitClippings(input);
            foreach (var clipping in entries)
            {
                yield return ParseClipping(clipping);
            }
        }

        private IEnumerable<string> SplitClippings(string input)
        {
            return input.Split("==========", StringSplitOptions.TrimEntries);
        }

        private Clipping ParseClipping(string input)
        {
            var lines = input.Split(Environment.NewLine, StringSplitOptions.TrimEntries);
            if (lines.Length < 4)
                return null;

            var titleLine = lines[0];
            var metaLine = lines[1];
            var content = string.Join(Environment.NewLine, lines[3..]);

            var author = ParseAuthorFromTitleLine(titleLine);
            var title = ParseTitleFromTitleLine(titleLine, author);

            return new Clipping(title, author, content, DateTime.MinValue);
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
                    //switch (level)
                    //{
                    //    case 1:
                    //        p = i + 1;
                    //        break;
                    //    case 2:
                    //        result.Add('"' + input.Substring(p, i - p) + '"');
                    //        p = i;
                    //        break;
                    //}
                }
                else if (input[i] == ')')
                    if (--level == 0)
                        result.Add(input.Substring(p, i - p));
            }

            return result;
        }
    }
}