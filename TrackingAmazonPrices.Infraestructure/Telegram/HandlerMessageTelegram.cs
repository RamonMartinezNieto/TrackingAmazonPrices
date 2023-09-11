using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TrackingAmazonPrices.Application.ApplicationFlow;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Enums;
using TrackingAmazonPrices.Domain.Exceptions;

namespace TrackingAmazonPrices.Infraestructure.Telegram;

public class HandlerMessageTelegram : IMessageHandler, IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HandlerMessageTelegram> _logger;
    private IControllerMessage _controllerMessage;

    public HandlerMessageTelegram(
        ILogger<HandlerMessageTelegram> logger,
        IBotClient<ITelegramBotClient> botClient)
    {
        _botClient = botClient.BotClient;
        _logger = logger;
    }

    public IControllerMessage SetControllerMessage(IControllerMessage controllerMessage)
    {
        if (IsValidController(controllerMessage))
        {
            _controllerMessage = controllerMessage;
            return _controllerMessage;
        }

        throw new InvalidControllerException();
    }

    public Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (IsValidController(_controllerMessage))
            return Task.FromException(_controllerMessage.HandlerError(exception));

        throw new InvalidControllerException();
    }

    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        if (!IsValidController(_controllerMessage))
            throw new InvalidControllerException();

        await Task.Run(() => _controllerMessage.HandlerMessage(update), cancellationToken);
    }

    public bool IsValidMessage<TMessage>(TMessage typeMessage)
    {
        return typeMessage is Update updateMessage &&
                updateMessage.Type is UpdateType.Message;
    }

    public bool IsCallBackQuery<TMessage>(TMessage typeMessage)
        => typeMessage is Update updateMessage
           && updateMessage.CallbackQuery is { };

    public string GetMessage<TMessage>(TMessage objectMessage)
    {
        if (objectMessage is Update updateMessage && IsValidMessage(updateMessage))
        {
            return updateMessage.Message.Text;
        }

        return string.Empty;
    }

    public async Task<bool> SentMessage(object objectMessage, string textMessage)
    {
        if (objectMessage is not Update update)
        {
            _logger.LogError("InvalidObjectMessage SentMessage");
            throw new ArgumentException("invalid objectMessage, this is not Update for telegram client");
        }

        if (update.Message is not { } message)
            return false;

        var result = await _botClient
            .SendTextMessageAsync(
                 chatId: message.Chat.Id,
                 text: textMessage,
                 disableNotification: true,
                 parseMode: ParseMode.MarkdownV2);

        return result != null;
    }

    public async Task<bool> SentInlineKeyboardMessage(
        object objectMessage,
        string textMessage,
        object menu)
    {
        if (objectMessage is not Update update)
        {
            _logger.LogError("InvalidObjectMessage SentMessage");
            throw new ArgumentException("invalid objectMessage, this is not Update for telegram client");
        }

        if (update.Message is not { } message)
            return false;

        var result = await _botClient
            .SendTextMessageAsync(
                 chatId: message.Chat.Id,
                 text: textMessage,
                 replyMarkup: (InlineKeyboardMarkup)menu,
                 disableNotification: true,
                 parseMode: ParseMode.MarkdownV2);

        return result != null;
    }

    private static bool IsValidController(IControllerMessage controllerMessage)
          => controllerMessage is not null;

    public long GetChatId(object objectMessage)
    {
        if (objectMessage is Update updateMessage &&
            updateMessage.Message is { } message)
        {
            return message.Chat.Id;
        }
        return default;
    }

    public MessageTypes GetTypeMessage(object objectMessage)
    {
        if (TryCastUpdate(objectMessage, out Update updateMessage))
        {
            return updateMessage switch
            {
                { Message: { } } => MessageTypes.Command,
                { CallbackQuery: { } } => MessageTypes.CallbackQuery,
                _ => MessageTypes.Nothing
            };
        }
        return MessageTypes.Nothing;
    }

    private static bool TryCastUpdate(object typeMessage, out Update updateMessage)
    {
        updateMessage = typeMessage as Update;
        return updateMessage != null;
    }
}