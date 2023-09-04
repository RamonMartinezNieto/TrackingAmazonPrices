using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TrackingAmazonPrices.Application.ApplicationFlow;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Infraestructure.Handlers;

namespace TrackingAmazonPrices.Infraestructure.Services;

public class MessageCommunicationTelegram : IComunicationHandler
{
    private readonly ILogger<MessageCommunicationTelegram> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IMessageHandler _handlerMessage;
    private readonly IControllerMessage _controllerMessage;

    public MessageCommunicationTelegram(
        ILogger<MessageCommunicationTelegram> logger,
        IBotClient<ITelegramBotClient> client,
        IMessageHandler handlerMessage,
        IControllerMessage controllerMessage)
    {
        _logger = logger;
        _botClient = client.BotClient;
        _handlerMessage = handlerMessage;
        _controllerMessage = controllerMessage;
    }

    public IMessageHandler StartComunication()
    {
        if (_handlerMessage is not HandlerMessageTelegram handler)
            throw new ArgumentException("The handler is not valid for bot telegram");

        _handlerMessage.SetControllerMessage(_controllerMessage);

        ReceiverOptions options = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        _logger.LogInformation("Starting communication with telegram.");

        using CancellationTokenSource cts = new();

        _botClient.StartReceiving(
                updateHandler: handler,
                receiverOptions: options,
                cancellationToken: cts.Token
            );

        _logger.LogInformation("Bot listening...");

        return _handlerMessage;
    }
}