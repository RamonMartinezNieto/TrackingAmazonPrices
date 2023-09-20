using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot;
using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Application.Callbacks;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Configurations;
using TrackingAmazonPrices.Infraestructure.Callbacks;
using TrackingAmazonPrices.Infraestructure.Commands;
using TrackingAmazonPrices.Infraestructure.MongoDataBase;
using TrackingAmazonPrices.Infraestructure.Telegram;

namespace TrackingAmazonPrices.Infraestructure;

[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    public static IServiceCollection AddConfigure(this IServiceCollection services)
    {
        services.Configure<BotConfig>(options =>
        {
            options.Token = Environment.GetEnvironmentVariable("TrackingAmazonBotToken");
        });

        services.Configure<SheetConfiguration>(options =>
        {
            options.SheetId = "16t9X1i4SpNOP-gAYoARL54Mcn5DeiLEjLNlg2oAwtWU";
            options.PathCredentials = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ILiteralsClient, SheetsLiteralsClient>()
                .AddSingleton<ILiteralsService, LiteralsServiceSheets>()
                .AddSingleton<IBotClient<ITelegramBotClient>, BotClientTelegram>()
                .AddSingleton<ICommandManager, CommandManager>()
                .AddSingleton<ICallbackManager, CallbackManager>()
                .AddSingleton<IMessageHandler, HandlerMessageTelegram>()
                .AddSingleton<IComunicationHandler, MessageCommunicationTelegram>();

        return services;
    }

    public static IServiceCollection AddDatabaseConnections(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("TrackingAmazonPrices.Atlas.ConnectionString");

        services.AddTransient<IMongoClient>(x => new MongoClient(connectionString))
                .AddSingleton<IDatabaseUserService, MongoUserService>();

        return services;
    }

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddTransient<ICommand, NullCommand>()
                .AddTransient<ICommand, StartCommand>()
                .AddTransient<ICommand, LanguageCommand>()
                .AddTransient<ICommand, DeleteUserCommand>();

        return services;
    }

    public static IServiceCollection AddCallbacks(this IServiceCollection services)
    {
        services.AddTransient<ICallback, NullCallback>()
                .AddTransient<ICallback, LanguageCallback>()
                .AddTransient<ICallback, DeleteUserCallback>();

        return services;
    }
}