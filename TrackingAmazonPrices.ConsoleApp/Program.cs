using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrackingAmazonPrices.Application.ApplicationFlow;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Configurations;
using TrackingAmazonPrices.Infraestructure.Handlers;
using TrackingAmazonPrices.Infraestructure.Services;
using Telegram.Bot;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Infraestructure.Commands;
using TrackingAmazonPrices.Application.Command;

namespace TrackingAmazonPrices.ConsoleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json")
            .Build();


        var _host = Host.CreateDefaultBuilder().ConfigureServices(
            services =>
            {
                services.AddLogging();
                services.AddMemoryCache();

                services.Configure<BotConfig>(options =>
                {
                    options.Token = Environment.GetEnvironmentVariable("TrackingAmazonBotToken");
                });

                services.AddSingleton<IBotClient<ITelegramBotClient>, BotClientTelegram>();
                services.AddSingleton<ICommandManager, CommandManager>();

                services.AddSingleton<IMessageHandler, HandlerMessageTelegram>();
                services.AddSingleton<IControllerMessage, ControllerMessages>();
                services.AddSingleton<IComunicationHandler, MessageCommunicationTelegram>();

                //commands
                services.AddTransient<ICommand, StartCommand>();
                services.AddTransient<ICommand, TestCommand>();

            }).Build();

        var app = _host.Services.GetRequiredService<IComunicationHandler>();
        IMessageHandler handler = app.StartComunication();

        Thread.Sleep(Timeout.Infinite);
    }
}