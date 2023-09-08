using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class StartCommand : ICommand
{
    public Steps NextStep { get; private set; }

    private readonly ILogger<StartCommand> _logger;
    private readonly IMessageHandler _messageHandler;

    public StartCommand(
        ILogger<StartCommand> logger,
        IMessageHandler messageHandler)
    {
        _logger = logger;
        _messageHandler = messageHandler;
        NextStep = Steps.Nothing;
    }

    public async Task<bool> ExecuteAsync(object objectMessage)
    {
        _logger.LogWarning("This is an start command");
        bool result = await _messageHandler.SentMessage(objectMessage, string.Format("{0} Choise language", TelegramEmojis.Question));
        NextStep = Steps.Test; 
        return result;
    }
}
