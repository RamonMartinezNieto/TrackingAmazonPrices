using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class StartCommand : ICommand
{
    private readonly ILogger<StartCommand> _logger;
    private readonly IMessageHandler _messageHandler;

    public StartCommand(
        ILogger<StartCommand> logger,
        IMessageHandler messageHandler)
    {
        _logger = logger;
        _messageHandler = messageHandler;
    }

    public Task ExecuteAsync(object objectMessage, CancellationToken cancellationToken)
    {
        _logger.LogWarning("This is an start command");
        _messageHandler.SentMessage(objectMessage, "Start Message", cancellationToken);
        return Task.CompletedTask;
    }
}
