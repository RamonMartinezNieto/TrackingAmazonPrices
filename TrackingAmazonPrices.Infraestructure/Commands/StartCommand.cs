using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Entities;
using TrackingAmazonPrices.Domain.Enums;
using TrackingAmazonPrices.Infraestructure.Telegram;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class StartCommand : ICommand
{
    public Steps NextStep { get; private set; } = Steps.Nothing;

    private readonly ILogger<StartCommand> _logger;
    private readonly IMessageHandler _messageHandler;
    private readonly ILiteralsService _literalsService;
    private readonly IDatabaseUserService _userService;

    public StartCommand(
        ILogger<StartCommand> logger,
        IMessageHandler messageHandler,
        ILiteralsService literalsService,
        IDatabaseUserService userService)
    {
        _logger = logger;
        _messageHandler = messageHandler;
        _literalsService = literalsService;
        _userService = userService;
    }

    public async Task<bool> ExecuteAsync(object objectMessage)
    {
        _logger.LogInformation("Start command");

        User user = await _messageHandler.GetUser(objectMessage);
        var userLang = await _userService.GetLanguage(user.UserId);

        if (userLang == 0)
        {
            userLang = user.Language.LanguageCode;
        }

        var firstResult = await _messageHandler.SentMessage(
            objectMessage,
            string.Format("{0} {1}",
                await _literalsService.GetAsync(userLang, Literals.Welcome),
                TelegramEmojis.SMILE)
            );

        bool result = false;
        if (firstResult)
        {
            _logger.LogInformation("Sent menu to choice language");

            var menu = UtilsTelegramMessage.CreateMenu(
                UtilsTelegramMessage.GetMenuLanguageRows());

            result = await _messageHandler.SentInlineKeyboardMessage(
                objectMessage,
                string.Format("{0} {1}",
                    TelegramEmojis.QUESTIONMARK,
                    await _literalsService.GetAsync(userLang, Literals.SelectLan)),
                menu);
        }

        return result;
    }
}