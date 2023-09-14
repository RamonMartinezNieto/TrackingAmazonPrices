using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Configurations;
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

        services.Configure<DatabaseConfig>(options =>
        {
            options.User = Environment.GetEnvironmentVariable("TrackingAmazonPrices.Atlas.User");
            options.Host = Environment.GetEnvironmentVariable("TrackingAmazonPrices.Atlas.Host");
            options.Protocol = Environment.GetEnvironmentVariable("TrackingAmazonPrices.Atlas.Protocol");
            options.Password = Environment.GetEnvironmentVariable("TrackingAmazonPrices.Atlas.Password");
            options.Database = Environment.GetEnvironmentVariable("TrackingAmazonPrices.Atlas.Database");
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
        services.AddTransient<IDatabaseHandler, MongoDatabaseHandler>();
        services.AddTransient<MongoConnection>();
        services.AddSingleton<IDatabaseUserHandler, MongoUserDatabaseHandler>();

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