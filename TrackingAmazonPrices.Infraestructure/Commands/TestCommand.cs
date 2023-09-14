using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Entities;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class TestCommand : ICommand
{
    public Steps NextStep { get; private set; }
    
    private readonly IDatabaseUserService _databaseHandler;
    private readonly ILogger<TestCommand> _logger;
    private readonly IMessageHandler _messageHandler;

    public TestCommand(
        ILogger<TestCommand> logger,
        IMessageHandler messageHandler,
        IDatabaseUserService databaseHandler)
    {
        _logger = logger;
        _messageHandler = messageHandler;
        _databaseHandler = databaseHandler;
        NextStep = Steps.Nothing;
    }

    public async Task<bool> ExecuteAsync(object objectMessage)
    {
        bool result = false;

        _logger.LogWarning("This is an TEST command");

        User user = _messageHandler.GetUser(objectMessage);

        var saved = await _databaseHandler.SaveUserAsync(user);
        
        if (saved) { 
            result = await _messageHandler.SentMessage(objectMessage, "TEST Message");
            NextStep = Steps.Nothing;
        }

        return result;
    }
}