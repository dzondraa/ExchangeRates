using ExchangeRates.Filters;
using ExchangeRates.Proxies;
using ExchangeRates.Services;
using Microsoft.AspNetCore.Builder;

namespace ExchangeRates
{
    public static class Startup
    {
        public static WebApplication InitializeApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            Configure(app);
            return app;
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient("ExchangeApi", httpClient =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Api:BaseUrl"));
            });
            builder.Services.AddScoped<IExchangeRateHttpProxy, ExchangeRateHttpProxy>();
            builder.Services.AddScoped<IHistoryService, HistoryService>();

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

        }

        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

        }
    }
}
