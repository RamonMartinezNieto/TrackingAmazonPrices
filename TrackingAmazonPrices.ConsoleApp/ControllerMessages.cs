using TrackingAmazonPrices.Application.ApplicationFlow;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;

namespace TrackingAmazonPrices.ConsoleApp;

public class ControllerMessages : IControllerMessage
{
    private readonly ILogger<ControllerMessages> _logger;
    private readonly ICommandManager _commandManager;
    private readonly IMessageHandler _handlerMessage;

    public Func<Exception, Exception> HandlerError { get; }
    public Action<object> HandlerMessage { get; }

    public ControllerMessages(
        ILogger<ControllerMessages> logger,
        ICommandManager commandManager,
        IMessageHandler handlerMessage)
    {
        _logger = logger;
        _commandManager = commandManager;
        _handlerMessage = handlerMessage;

        HandlerError = HandleExceptionImp;
        HandlerMessage = HandlerMessageImp;
    }

    public void HandlerMessageImp(object objectMessage)
    {
        _logger.LogInformation("recibiendo un mensaje");

        using CancellationTokenSource cts = new();

        if (!_handlerMessage.IsValidMessage(objectMessage))
            throw new ArgumentException("Object Message not valid");

        var message = _handlerMessage.GetMessage(objectMessage);
        if (_commandManager.IsCommand(message))
        {
            _logger.LogWarning("YES");

            var command = _commandManager.GetCommand(message);
            command.ExecuteAsync(objectMessage, cts.Token);
        }

        _handlerMessage.PrintMessage(objectMessage);
        _handlerMessage.SentMessage(objectMessage, "hooola", cts.Token);
    }

    public Exception HandleExceptionImp(Exception exception)
    {
        _logger.LogError(exception, "Error in HandlerMessageTelegram");
        return exception;
    }
}