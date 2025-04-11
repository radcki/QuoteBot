using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using QuotBot.Core.Model;

namespace QuotBot.Core.Services.PushoverPublisher
{
    public class PushoverPublisherServiceConfiguration
    {
        public static string SectionName = "PushoverPublisherService";
        public string ApiKey { get; set; }
        public string UserKey { get; set; }
        public string PublishUrl { get; set; } = @"https://api.pushover.net/1/messages.json";
    }

    public class PushoverPublisherService(PushoverPublisherServiceConfiguration configuration)
    {
        private readonly HttpClient _httpClient = new();

        public async Task PublishNotification(string message, string? title)
        {
            var request = new PushoverRequest()
                          {
                              Token = configuration.ApiKey,
                              User = configuration.UserKey,
                              Message = message,
                              Title = title
                          };
            var response = await _httpClient.PostAsJsonAsync(configuration.PublishUrl, request);
        }
    }

    internal class PushoverRequest
    {
        public required string Token { get; set; }
        public required string User { get; set; }
        public required string Message { get; set; }
        public string? Title { get; set; }
    }
}