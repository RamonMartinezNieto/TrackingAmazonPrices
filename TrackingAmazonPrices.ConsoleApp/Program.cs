using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TrackingAmazonPrices.Domain.Configurations;
using TrackingAmazonPrices.Application.ApplicationFlow;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Infraestructure.Services;
using TrackingAmazonPrices.Infraestructure.Handlers;
using TrackingAmazonPrices.Infraestructure.StartCommunication;

namespace TrackingAmazonPrices.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>(true)
            .Build();

        IHost _host = Host.CreateDefaultBuilder().ConfigureServices(
            services => {

                services.AddLogging();
                services.AddMemoryCache();


         

                services.Configure<BotConfig>(options => 
                {
                    options.Token = "5806167847:AAEUEDb-2MOAjikJ_-4ctB2XeoE5LrWlvGw";
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
