using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class TestCommand : ICommand
{
    private readonly ILogger<TestCommand> _logger;
    private readonly IMessageHandler _messageHandler;
    
    public TestCommand(
        ILogger<TestCommand> logger,
        IMessageHandler messageHandler)
    {
        _logger = logger;
        _messageHandler = messageHandler;
    }

    public Task ExecuteAsync(object objectMessage, CancellationToken cancellationToken)
    {
        _logger.LogWarning("This is an TEST command");
        _messageHandler.SentMessage(objectMessage, "TEST Message", cancellationToken);
        return Task.CompletedTask;
    }
}
