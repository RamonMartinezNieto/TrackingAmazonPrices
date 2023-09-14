using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Telegram.Bot;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Infraestructure.Commands;
using TrackingAmazonPrices.Infraestructure.MongoDataBase;
using TrackingAmazonPrices.Infraestructure.Telegram;

namespace TrackingAmazonPrices.Infraestructure;

public static class ConfigureServices
{
    public static IServiceCollection AddConfigure(this IServiceCollection services)
    {
        services.Configure<BotConfig>(options =>
        {
            options.Token = Environment.GetEnvironmentVariable("TrackingAmazonBotToken");
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IBotClient<ITelegramBotClient>, BotClientTelegram>();
        services.AddSingleton<ICommandManager, CommandManager>();
        services.AddSingleton<IMessageHandler, HandlerMessageTelegram>();
        services.AddSingleton<IComunicationHandler, MessageCommunicationTelegram>();
        services.AddSingleton<IPoolingCommands, PoolingCommands>();

        return services;
    }

    public static IServiceCollection AddDatabaseConnections(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("TrackingAmazonPrices.Atlas.ConnectionString");
        services.AddTransient<IMongoClient>(x => new MongoClient(connectionString));
        services.AddSingleton<IDatabaseUserService, MongoUserService>();

        return services;
    }

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddTransient<ICommand, NullCommand>();
        services.AddTransient<ICommand, StartCommand>();
        services.AddTransient<ICommand, TestCommand>();
        return services;
    }
}