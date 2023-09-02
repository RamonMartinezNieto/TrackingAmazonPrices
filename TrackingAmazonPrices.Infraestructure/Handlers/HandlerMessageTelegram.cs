using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TrackingAmazonPrices.Infraestructure.Handlers;

public class HandlerMessageTelegram : HandlerMessage, IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HandlerMessageTelegram> _logger;

    public HandlerMessageTelegram(
        ILoggerFactory logger,
        ITelegramBotClient botClient,
        Func<Exception, Exception> handlerError,
        Action<object> handlerMessage)
        : base(handlerError, handlerMessage)
    {
        _botClient = botClient;
        _logger = logger.CreateLogger<HandlerMessageTelegram>();
    }

    public Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        return Task.FromException(_handlerError(exception));
    }

    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        await Task.Run(() => _handlerMessage(update), cancellationToken);
    }

    public override bool IsValidMessage<TMessage>(TMessage update)
    {
        if (update is not Update updateMessage)
            return false;

        if (updateMessage.Message is not { } message)
            return false;

        if (message.Text is not { } messageText)
            return false;

        return true;
    }

    public override void PrintMessage(object objectMessage)
    {
        if (IsValidMessage(objectMessage))
        {
            Update update = (Update)objectMessage;
            _logger.LogInformation(update.Message.Text);
        }
    }

    public override async Task SentMessage(object objectMessage, string text, CancellationToken cts)
    {
        if (objectMessage is not Update update)
            throw new ArgumentException("invalid objectMessage, this is not Update for telegram client");

        if (update.Message is not { } message)
            return;

        await _botClient.SendTextMessageAsync(
                 chatId: message.Chat.Id,
                 text: text,
                 disableNotification: true,
                 parseMode: ParseMode.MarkdownV2,
                 cancellationToken: cts);
    }
}