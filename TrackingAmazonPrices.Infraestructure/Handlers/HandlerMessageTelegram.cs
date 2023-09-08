using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TrackingAmazonPrices.Application.ApplicationFlow;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;

namespace TrackingAmazonPrices.Infraestructure.Handlers;

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

    public void SetControllerMessage(IControllerMessage controllerMessage)
    {
        _controllerMessage = controllerMessage;
    }

    public Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (!IsValidController())
            throw new ArgumentNullException("ControllerMessage is not defined, call method SetControllerMessage");

        return Task.FromException(_controllerMessage.HandlerError(exception));
    }

    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        if (!IsValidController())
            throw new ArgumentNullException("ControllerMessage is not defined, call method SetControllerMessage");

        await Task.Run(() => _controllerMessage.HandlerMessage(update), cancellationToken);
    }

    public bool IsValidMessage<TMessage>(TMessage update)
    {
        return update is Update updateMessage &&
                updateMessage.Message is { } message &&
                message.Text is { };
    }

    public bool IsCallBackQuery<TMessage>(TMessage message)
    {
        var eso = message is Update updateMessage &&
                updateMessage.CallbackQuery is { };
        return eso;
    }

    public string GetMessage<TMessage>(TMessage update)
    {
        if (update is Update updateMessage &&
            updateMessage.Message is { } message &&
            message.Text is { } messageText)
        {
            return messageText;
        }
        return string.Empty;
    }

    public async Task<bool> SentMessage(object objectMessage, string text)
    {
        if (objectMessage is not Update update)
        {
            _logger.LogError("InvalidObjectMessage SentMessage");
            throw new ArgumentException("invalid objectMessage, this is not Update for telegram client");
        }
        if (update.Message is not { } message)
            return false;

        var result = await _botClient.SendTextMessageAsync(
                 chatId: message.Chat.Id,
                 text: text,
                 disableNotification: true,
                 parseMode: ParseMode.MarkdownV2);

        return result != null;
    }

    public async Task<bool> SentInlineMenuMessage(object objectMessage, string text)
    {
        if (objectMessage is not Update update)
        {
            _logger.LogError("InvalidObjectMessage SentMessage");
            throw new ArgumentException("invalid objectMessage, this is not Update for telegram client");
        }
        if (update.Message is not { } message)
            return false;

        var result = await _botClient.SendTextMessageAsync(
                 chatId: message.Chat.Id,
                 text: text,
                 replyMarkup: CreateMenu(new string[] { "asda", "yterty" }),
                 disableNotification: true,
                 parseMode: ParseMode.MarkdownV2);

        return result != null;
    }

    public static ReplyKeyboardMarkup CreateMenu(params string[] textItemsRows)
    {
        string[] rows = new string[textItemsRows.Length];

        for (int i = 0; i <= textItemsRows.Length; i++)
        {
            rows[i] = textItemsRows[i];
        }

        ReplyKeyboardMarkup replyKeyboard = new string[][] { rows };

        return replyKeyboard;
    }

    private bool IsValidController()
        => _controllerMessage is not null;

    public long GetChatId(object objectMessage)
    {
        if (objectMessage is Update updateMessage &&
            updateMessage.Message is { } message)
        {
            return message.Chat.Id;
        }
        return default;
    }
}