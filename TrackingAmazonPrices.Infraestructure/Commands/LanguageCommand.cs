using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Domain;
using TrackingAmazonPrices.Domain.Entities;
using TrackingAmazonPrices.Domain.Enums;
using TrackingAmazonPrices.Infraestructure.Telegram;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class LanguageCommand : ICommand
{
    private readonly ILogger<LanguageCommand> _logger;
    private readonly IMessageHandler _messageHandler;
    private readonly ILiteralsService _literalsService;

    public LanguageCommand(
        ILogger<LanguageCommand> logger,
        IMessageHandler messageHandler,
        ILiteralsService literalsService)
    {
        _logger = logger;
        _messageHandler = messageHandler;
        _literalsService = literalsService;
    }

    public async Task<bool> ExecuteAsync(object objectMessage)
    {
        _logger.LogInformation("Language command");

        var menu = UtilsTelegramMessage.CreateMenu(
            UtilsTelegramMessage.GetMenuLanguageRows());

        User user = await _messageHandler.GetUser(objectMessage);

        LanguageType lang = user == null ? LanguageType.Default : user.Language.LanguageCode;

        return await _messageHandler.SentInlineKeyboardMessage(
            objectMessage,
            string.Format("{0} {1}",
                TelegramEmojis.QUESTIONMARK,
                await _literalsService.GetAsync(lang, Literals.SelectLan)),
                menu);
    }
}