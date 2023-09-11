using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Domain.Enums;
using TrackingAmazonPrices.Infraestructure.Telegram;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class StartCommand : ICommand
{
    public Steps NextStep { get; private set; }

    private readonly ILogger<StartCommand> _logger;
    private readonly IMessageHandler _messageHandler;

    public StartCommand(
        ILogger<StartCommand> logger,
        IMessageHandler messageHandler)
    {
        _logger = logger;
        _messageHandler = messageHandler;
        NextStep = Steps.Nothing;
    }

    public async Task<bool> ExecuteAsync(object objectMessage)
    {
        _logger.LogInformation("Start command");

        List<string[,]> menuRows = new()
        {
            new string[2, 2] { 
                { "ES " + TelegramEmojis.SPAINT_FLAG, "ESP" }, 
                { "EN " + TelegramEmojis.GB_FLAG, "EN" } },
        };

        var menu = UtilsTelegramMessage.CreateMenu(menuRows);

        bool firstMessage = await _messageHandler.SentMessage(
            objectMessage, 
            string.Format("Welcom to Tracking Amazon Prices {0}", TelegramEmojis.SMILE));

        bool result = false;
        if (firstMessage) { 
            result = await _messageHandler.SentInlineKeyboardMessage(
                objectMessage, 
                string.Format("{0} Select a language", TelegramEmojis.QUESTIONMARK),
                menu);
        }

        if (result) NextStep = Steps.Test;

        return result;
    }
}