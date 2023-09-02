using TrackingAmazonPrices.Application.Handlers;

namespace TrackingAmazonPrices.ConsoleApp;

public class ControllerMessages
{
    private readonly ILogger<ControllerMessages> _logger;
    private IHandlerMessage _handlerMessage;

    public ControllerMessages(ILogger<ControllerMessages> logger)
    {
        _logger = logger;
    }

    public void SetHandler(IHandlerMessage handlerMessage)
    {
        _handlerMessage = handlerMessage;
    }

    public void HandlerMessage(object objectMessage)
    {
        _logger.LogInformation("recibiendo un mensaje");

        using CancellationTokenSource cts = new();

        if (!_handlerMessage.IsValidMessage(objectMessage))
            throw new ArgumentException("Object Message not valid");

        _handlerMessage.PrintMessage(objectMessage);

        _handlerMessage.SentMessage(objectMessage, "hooola", cts.Token);
    }

    public Exception HandleException(Exception exception)
    {
        _logger.LogError(exception, "Error in HandlerMessageTelegram");
        return exception;
    }
}