using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Enums;
using TrackingAmazonPrices.Infraestructure.Telegram;
using static TrackingAmazonPrices.Domain.Definitions.CallbackMessages;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class DeleteUserCommand : ICommand
{
    private readonly ILogger<DeleteUserCommand> _logger;
    private readonly IDatabaseUserService _userService;
    private readonly IMessageHandler _handlerMessage;
    private readonly ILiteralsService _literalsService;

    public DeleteUserCommand(
        ILogger<DeleteUserCommand> logger,
        IDatabaseUserService userService,
        IMessageHandler handlerMessage,
        ILiteralsService literalsService)
    {
        _logger = logger;
        _userService = userService;
        _handlerMessage = handlerMessage;
        _literalsService = literalsService;
    }

    public async Task<bool> ExecuteAsync(object objectMessage)
    {
        var user = await _handlerMessage.GetUser(objectMessage);

        if (!(await _userService.UserExists(user.UserId))) 
        {
            return await _handlerMessage.SentMessageAsync(
                objectMessage,
                 await _literalsService.GetAsync(Literals.NoUser));
        }

        var userLanguage = user.Language.LanguageCode;

        List<string[,]> menuRows = new()
        {
            new string[2, 2] 
            {
                { await _literalsService.GetAsync(userLanguage, Literals.Yes), GetCallback(DELETE_USER_CALLBACK, "yes" ) },
                { await _literalsService.GetAsync(userLanguage, Literals.No), GetCallback(DELETE_USER_CALLBACK, "no" ) } 
            }
        };

        var menu = UtilsTelegramMessage.CreateMenu(menuRows);

        _logger.LogInformation("Send menu to delete user");

        return await _handlerMessage.SentInlineKeyboardMessage(
            objectMessage,
            string.Format("{0} {1}",
                TelegramEmojis.QUESTIONMARK,
                await _literalsService.GetAsync(userLanguage, Literals.QuestionIfSure)),
                menu);
    }
}
