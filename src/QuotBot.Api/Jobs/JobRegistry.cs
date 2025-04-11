using Hangfire;

namespace QuotBot.Api.Jobs
{
    public class JobRegistryConfiguration
    {
        public static string SectionName = "Jobs";
        public JobConfiguration SendRandomQuoteAsNotification { get; set; }

        public class JobConfiguration
        {
            public bool IsEnabled { get; set; }
            public string Cron { get; set; }
        }
    }

    public class JobRegistry(JobRegistryConfiguration configuration, IServiceProvider serviceProvider)
    {
        public void RegisterJobs()
        {
            if (configuration.SendRandomQuoteAsNotification.IsEnabled)
            {
                RecurringJob.AddOrUpdate("SendRandomQuoteAsNotification",
                                         () => serviceProvider.CreateScope().ServiceProvider.GetRequiredService<SendRandomQuoteAsNotificationJob>().Execute(),
                                         configuration.SendRandomQuoteAsNotification.Cron);
            }
        }
    }
}