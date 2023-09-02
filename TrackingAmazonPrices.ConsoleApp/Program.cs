using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrackingAmazonPrices.Application.ApplicationFlow;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Configurations;
using TrackingAmazonPrices.Infraestructure.Handlers;
using TrackingAmazonPrices.Infraestructure.Services;
using TrackingAmazonPrices.Infraestructure.Communications;

namespace TrackingAmazonPrices.ConsoleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json")
            .Build();

        IHost _host = Host.CreateDefaultBuilder().ConfigureServices(
            services =>
            {
                services.AddLogging();
                services.AddMemoryCache();

                var eso = Environment.GetEnvironmentVariable("TrackingAmazonBotToken");

                services.Configure<BotConfig>(options =>
                {
                    options.Token = Environment.GetEnvironmentVariable("TrackingAmazonBotToken");
                });

                services.AddSingleton<IBotProvider, BotProviderTelegram>();
                services.AddSingleton<IBotClient, BotClientTelegram>();
                services.AddSingleton<IComunicationHandler, MessageCommunicationTelegram>();
                services.AddSingleton<IStartComunication, StartCommunication>();
                services.AddSingleton<ControllerMessages>();
            })
        .Build();

        var controllerMessage = _host.Services.GetRequiredService<ControllerMessages>();
        var app = _host.Services.GetRequiredService<IStartComunication>();

        var handler = app.Start<HandlerMessageTelegram>(
            controllerMessage.HandleException,
            controllerMessage.HandlerMessage);

        controllerMessage.SetHandler(handler);

        Thread.Sleep(Timeout.Infinite);
    }
}