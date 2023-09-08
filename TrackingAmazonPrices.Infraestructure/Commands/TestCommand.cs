using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class TestCommand : ICommand
{
    public Steps NextStep { get; private set; }

    private readonly ILogger<TestCommand> _logger;
    private readonly IMessageHandler _messageHandler;

    public TestCommand(
        ILogger<TestCommand> logger,
        IMessageHandler messageHandler)
    {
        _logger = logger;
        _messageHandler = messageHandler;
        NextStep = Steps.Nothing;
    }

    public async Task<bool> ExecuteAsync(object objectMessage)
    {
        _logger.LogWarning("This is an TEST command");
        
        bool result = await _messageHandler.SentMessage(objectMessage, "TEST Message");
        NextStep = Steps.Nothing;

        return result;
    }
}
