using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Infraestructure;
using TrackingAmazonPrices.Application.ApplicationFlow;

namespace TrackingAmazonPrices.ConsoleApp;

internal static class Program
{
    private static void Main()
    {
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json")
            .Build();


        var _host = Host.CreateDefaultBuilder().ConfigureServices(
            services =>
            {
                services.AddLogging();
                services.AddMemoryCache();

                services.AddConfigure();
                services.AddServices();
                services.AddCommands();
                services.AddSingleton<IControllerMessage, ControllerMessages>();

            }).Build();

        var app = _host.Services.GetRequiredService<IComunicationHandler>();
        app.StartComunication();

        Thread.Sleep(Timeout.Infinite);
    }
}