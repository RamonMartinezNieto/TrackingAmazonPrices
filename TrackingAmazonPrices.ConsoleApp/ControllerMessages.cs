using TrackingAmazonPrices.Application.ApplicationFlow;
using TrackingAmazonPrices.Application.Callbacks;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Entities;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.ConsoleApp;

public class ControllerMessages : IControllerMessage
{
    private readonly ILogger<ControllerMessages> _logger;
    private readonly ICommandManager _commandManager;
    private readonly ICallbackManager _callbackManager;
    private readonly IMessageHandler _handlerMessage;
    private readonly IScraperService<AmazonObject> scr;

    public Func<Exception, Exception> HandlerError { get; }
    public Action<object> HandlerMessage { get; }

    public ControllerMessages(
        ILogger<ControllerMessages> logger,
        ICommandManager commandManager,
        ICallbackManager callbackManager,
        IMessageHandler handlerMessage)
    {
        _logger = logger;
        _commandManager = commandManager;
        _callbackManager = callbackManager;
        _handlerMessage = handlerMessage;
        this.scr = scr;
        HandlerError = HandleExceptionImp;
        HandlerMessage = HandlerMessageImp;
    }

    public void HandlerMessageImp(object objectMessage)
    {
        _logger.LogInformation("receiving message");

        var messageType = _handlerMessage.GetTypeMessage(objectMessage);

        switch (messageType)
        {
            case MessageTypes.Command:
                _ = ProcessCommand(objectMessage);
                break;

            case MessageTypes.CallbackQuery:
                _ = ProcessCallback(objectMessage);
                break;

            case MessageTypes.Url:
                ProcessUrl();
                break;

            default:
                _logger.LogWarning("Unsupported message type");
                break;
        }
    }

    private void ProcessUrl()
    {
        _logger.LogInformation("This is an URL :)");
    }

    private async Task ProcessCallback(object objectMessage)
    {
        if (_handlerMessage.IsCallBackQuery(objectMessage))
        {
            string message = _handlerMessage.GetCallbackMessage(objectMessage);
            string data = _callbackManager.GetData(message);

            ICallback callback = _callbackManager.GetCallback(message);

            _ = await callback.ExecuteAsync(objectMessage, data);
        }
    }

    private async Task ProcessCommand(object objectMessage)
    {
        if (_handlerMessage.IsValidMessage(objectMessage))
        {
            var message = _handlerMessage.GetMessage(objectMessage);

            ICommand command = _commandManager.GetCommand(message);

            _ = await command.ExecuteAsync(objectMessage);
        }
    }

    public Exception HandleExceptionImp(Exception exception)
    {
        _logger.LogError(exception, "Error in HandlerMessageTelegram");
        return exception;
    }
}