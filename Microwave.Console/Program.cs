
using DoverFueling.Core.Entity;
using DoverFueling.UI;
using DoverFueling.UI.Console;
using Microsoft.Extensions.Configuration;
using Microwave.Application.Services;
using Microwave.Infrastructure.Configuration;
using Microwave.Infrastructure.Validation;
using Serilog;

namespace DoverFueling
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);
            var configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            var cfg = new MicrowaveConfiguration();
            configuration.GetSection("Microwave").Bind(cfg);

            var validator = new MicrowaveInputValidator();
            var configValidation = validator.ValidateConfiguration(cfg);
            
            if (!configValidation.IsValid)
            {
                Log.Error($"Configuration validation failed: {configValidation.ErrorMessage}");
                Console.WriteLine($"Configuration Error: {configValidation.ErrorMessage}");
                return;
            }

            var device = new MicrowaveOvenHW();
            
            using var microwave = new MicrowaveOwen(Log.Logger, 
                new MicrowaveOvenService(device, cfg), 
                new UserNotification(cfg),
                validator);

            microwave.Start();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
