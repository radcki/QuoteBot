
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using QuotBot.Core.Data;
using QuotBot.Core.Services.KindleClippingsLoader;

namespace QuotBot.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();
            builder.Services.AddFastEndpoints();
            builder.Services.AddTransient<KindleClippingsFileQuoteLoader>();
            builder.Services.AddTransient<KindleClippingQuoteParser>();
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<IDatabaseContext, DatabaseContext>(options => options.UseSqlite("Data Source=db.dat"));

            var app = builder.Build();

            app.UseFastEndpoints();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.Run();
        }
    }
}
