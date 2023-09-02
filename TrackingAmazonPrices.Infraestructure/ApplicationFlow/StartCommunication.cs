using TrackingAmazonPrices.Application.ApplicationFlow;
using Microsoft.Extensions.Logging;
using TrackingAmazonPrices.Application.Services;
using System;
using TrackingAmazonPrices.Application.Handlers;
using Telegram.Bot;

namespace TrackingAmazonPrices.Infraestructure.StartCommunication;

public class StartCommunication : IStartComunication
{
    private readonly ILogger<StartCommunication> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IComunicationHandler _comunication;
    private readonly ITelegramBotClient _botClient;

    //Cambiar BOtClientTelegram por interfaz
    public StartCommunication(
        ILoggerFactory loggerFactory,
        IComunicationHandler comunication,
        IBotClient botClient)
    {
        _logger = loggerFactory.CreateLogger<StartCommunication>();
        _loggerFactory = loggerFactory;
        _comunication = comunication;
        _botClient = (ITelegramBotClient)botClient.BotClient;
    }

    public IHandlerMessage Start<THandlerMessage>(
        Func<Exception, Exception> handlerError,
        Action<object> handlerMessage
        )
        where THandlerMessage : IHandlerMessage
    {
        _logger.LogInformation("Starting comunication");

        _logger.LogWarning(typeof(THandlerMessage).ToString());

        object[] constructorArgs = { _loggerFactory, _botClient, handlerError, handlerMessage };
        var handler = (THandlerMessage)Activator.CreateInstance(typeof(THandlerMessage), constructorArgs);

        return _comunication.StartComunication(handler);
    }
}
