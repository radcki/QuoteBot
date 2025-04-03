using System.Text;

namespace QuotBot.Api.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<string> ReadAsStringAsync(this IFormFile file, CancellationToken cancellationToken)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0 && !cancellationToken.IsCancellationRequested)
                    result.AppendLine(await reader.ReadLineAsync(cancellationToken));
            }

            return result.ToString();
        }
    }
}