using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Application.Callbacks;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Infraestructure.Callbacks;

public class LanguageCallback : ICallback
{
    private readonly ILogger<LanguageCallback> _logger;
    private readonly IMessageHandler _messageHandler;
    private readonly ILiteralsService _literals;
    private readonly IDatabaseUserService _userService;

    public LanguageCallback(
        ILogger<LanguageCallback> logger,
        IMessageHandler messageHandler,
        ILiteralsService literals,
        IDatabaseUserService userService)
    {
        _logger = logger;
        _messageHandler = messageHandler;
        _literals = literals;
        _userService = userService;
    }

    public async Task<bool> ExecuteAsync(object objectMessage, string dataCallback)
    {
        _logger.LogInformation("CallbackQuery Update Language! :)");

        Domain.Entities.User user = await _messageHandler.GetUser(objectMessage);

        user.Language = dataCallback;

        var result = await _userService.SaveUserAsync(user);

        if (result)
        {
            string message = await _literals.GetAsync(user.Language.LanguageCode, Literals.UserUpdate);
            _ = await _messageHandler.AnswerdCallback(objectMessage, message);
            _ = await _messageHandler.SentMessageAsync(objectMessage, message);
        }

        if (result)
            _logger.LogInformation("User updated successfully: {UserName}:{UserId}", user.Name, user.UserId);
        else
            _logger.LogWarning("Can't send user: {UserName}:{UserId}", user.Name, user.UserId);

        return result;
    }
}