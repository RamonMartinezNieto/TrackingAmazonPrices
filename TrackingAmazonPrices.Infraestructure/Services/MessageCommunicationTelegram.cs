using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Infraestructure.Handlers;

namespace TrackingAmazonPrices.Infraestructure.Services;

public class MessageCommunicationTelegram : IComunicationHandler
{
    private readonly ILogger<MessageCommunicationTelegram> _logger;
    private readonly ITelegramBotClient _botClient;

    public MessageCommunicationTelegram(
        ILogger<MessageCommunicationTelegram> logger,
        IBotClient client)
    {
        _logger = logger;
        _botClient = (ITelegramBotClient)client.BotClient;
    }

    public IHandlerMessage StartComunication(IHandlerMessage handlerMessage)
    {
        if (handlerMessage is not HandlerMessageTelegram handler)
            throw new ArgumentException("The handler is not valid for bot telegram");

        ReceiverOptions options = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        _logger.LogInformation("Iniciando comunicación");

        using CancellationTokenSource cts = new();

        _botClient.StartReceiving(
                updateHandler: handler,
                receiverOptions: options,
                cancellationToken: cts.Token
            );

        _logger.LogInformation("Bot listening...");

        return handlerMessage;
    }
}