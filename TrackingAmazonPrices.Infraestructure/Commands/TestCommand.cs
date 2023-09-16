using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Entities;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class TestCommand : ICommand
{
    public Steps NextStep { get; private set; }

    private readonly IDatabaseUserService _databaseHandler;
    private readonly ILiteralsService _literalsService;
    private readonly ILogger<TestCommand> _logger;
    private readonly IMessageHandler _messageHandler;

    public TestCommand(
        ILogger<TestCommand> logger,
        IMessageHandler messageHandler,
        IDatabaseUserService databaseHandler,
        ILiteralsService literalsService)
    {
        _logger = logger;
        _messageHandler = messageHandler;
        _databaseHandler = databaseHandler;
        _literalsService = literalsService;
        NextStep = Steps.Nothing;
    }

    public async Task<bool> ExecuteAsync(object objectMessage)
    {
        bool result = false;

        _logger.LogWarning("This is an TEST command");

        User user = _messageHandler.GetUser(objectMessage);

        var saved = await _databaseHandler.SaveUserAsync(user);


        if (saved)
        {
            var message = await _literalsService.GetAsync(user.Language.LanguageCode, Literals.Test);
            result = await _messageHandler.SentMessage(objectMessage, message);
            NextStep = Steps.Nothing;
        }

        return result;
    }
}