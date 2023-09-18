using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using TrackingAmazonPrices.Application.ApplicationFlow;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Infraestructure;
using TrackingAmazonPrices.Shared.Logging;

namespace TrackingAmazonPrices.ConsoleApp;

[ExcludeFromCodeCoverage]
internal static class Program
{
    private static void Main()
    {
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json")
            .Build();

        var _host = Host.CreateDefaultBuilder()
            .ConfigureSerilog()
            .ConfigureServices(
                services =>
                {
                    services.AddLogging(logger => logger.AddSerilog());
                    services.AddMemoryCache();

                    services.AddConfigure();
                    services.AddServices();
                    services.AddDatabaseConnections();
                    services.AddCommands();
                    services.AddCallbacks();
                    services.AddSingleton<IControllerMessage, ControllerMessages>();
                })
            .Build();

        var app = _host.Services.GetRequiredService<IComunicationHandler>();
        app.StartComunication();

        Thread.Sleep(Timeout.Infinite);
    }
}