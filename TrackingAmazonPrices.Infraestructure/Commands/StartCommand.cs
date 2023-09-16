using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Domain.Entities;
using TrackingAmazonPrices.Infraestructure.Telegram;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class StartCommand : ICommand
{
    public Steps NextStep { get; private set; }

    private readonly ILogger<StartCommand> _logger;
    private readonly IMessageHandler _messageHandler;
    private readonly ILiteralsService _literalsService;

    public StartCommand(
        ILogger<StartCommand> logger,
        IMessageHandler messageHandler,
        ILiteralsService literalsService)
    {
        _logger = logger;
        _messageHandler = messageHandler;
        this._literalsService = literalsService;
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

        User user = _messageHandler.GetUser(objectMessage);
        var userLang = user.Language.LanguageCode;

        bool firstMessage = await _messageHandler.SentMessage(
            objectMessage,
            string.Format("{0} {1}", 
                await _literalsService.GetAsync(userLang, Literals.Welcome), 
                TelegramEmojis.SMILE)
            );

        bool result = false;
        if (firstMessage)
        {
            result = await _messageHandler.SentInlineKeyboardMessage(
                objectMessage,
                string.Format("{0} {1}", 
                    TelegramEmojis.QUESTIONMARK,
                    await _literalsService.GetAsync(userLang, Literals.SelectLan)),
                menu);
        }

        if (result) NextStep = Steps.Test;

        return result;
    }
}