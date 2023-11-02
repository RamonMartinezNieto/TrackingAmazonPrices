using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http.Headers;
using Telegram.Bot;
using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Application.Callbacks;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Configurations;
using TrackingAmazonPrices.Domain.Entities;
using TrackingAmazonPrices.Infraestructure.Amazon;
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
                .AddSingleton<IHtmlContentRepository, AmazonHtmlContentRepository>()
                .AddSingleton<IScraperService<AmazonObject>, AmazonScraperService>()
                .AddSingleton<IComunicationHandler, MessageCommunicationTelegram>();

        services.AddSingleton<IClientService, SheetsService>(service =>
        {
            using var stream = new FileStream(
                Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS"), 
                FileMode.Open,
                FileAccess.Read);

            var credential = (GoogleCredential.FromStream(stream))
                .CreateScoped(SheetsService.Scope.Spreadsheets);

            return new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        });

        return services;
    }

    public static IServiceCollection AddDatabaseConnections(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("TrackingAmazonPrices_Atlas_ConnectionString");

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
    
    public static IServiceCollection ConfigureClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient("AmazonClient", client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("Clients:AmazonClient"));
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("html/text"));
        });

        return services;
    }
}