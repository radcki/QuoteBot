using System.Reflection;
using FastEndpoints;
using Hangfire;
using Hangfire.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QuotBot.Api.Jobs;
using QuotBot.Core.Data;
using QuotBot.Core.Services.KindleClippingsLoader;
using QuotBot.Core.Services.PushoverPublisher;

namespace QuotBot.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                               .AddJsonFile("appsettings.json")
                               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                               .AddEnvironmentVariables()
                               .AddUserSecrets(typeof(Program).Assembly)
                               .Build();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();
            builder.Services.AddFastEndpoints();
            builder.Services.AddTransient<KindleClippingsFileQuoteLoader>();
            builder.Services.AddTransient<KindleClippingQuoteParser>();

            builder.Services.Configure<PushoverPublisherServiceConfiguration>(configuration.GetSection(PushoverPublisherServiceConfiguration.SectionName));
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<PushoverPublisherServiceConfiguration>>().Value);

            builder.Services.Configure<JobRegistryConfiguration>(configuration.GetSection(JobRegistryConfiguration.SectionName));
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JobRegistryConfiguration>>().Value);

            builder.Services.AddTransient<SendRandomQuoteAsNotificationJob>();

            builder.Services.AddTransient<PushoverPublisherService>();
            builder.Services.AddOpenApi();

            var dbFilePath = Path.Combine(configuration.GetValue<string>("DataPath") ?? "", "data.db");
            builder.Services.AddDbContext<IDatabaseContext, DatabaseContext>(options => options.UseSqlite($"Data Source={dbFilePath}"));

            builder.Services.AddCors(options =>
                                     {
                                         options.AddDefaultPolicy(policy =>
                                                                  {
                                                                      policy.WithOrigins("http://localhost:3000")
                                                                            .AllowAnyMethod()
                                                                            .AllowAnyHeader();
                                                                  });
                                     });
            builder.Services.AddHangfire(configuration => configuration
                                                         .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                                         .UseSimpleAssemblyNameTypeSerializer()
                                                         .UseRecommendedSerializerSettings()
                                                         .UseInMemoryStorage());
            builder.Services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(10));
            JobStorage.Current = new InMemoryStorage();
            builder.Services.AddSingleton<JobRegistry>();


            var app = builder.Build();

            app.UseHangfireDashboard();
            app.UseFastEndpoints();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapHangfireDashboard();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthorization();
            app.Services.GetRequiredService<JobRegistry>().RegisterJobs();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.Run();
        }
    }
}