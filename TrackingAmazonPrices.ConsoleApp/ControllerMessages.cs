using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrackingAmazonPrices.Application.ApplicationFlow;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Domain.Enums;
using TrackingAmazonPrices.Infraestructure.Commands;

namespace TrackingAmazonPrices.ConsoleApp;

public class ControllerMessages : IControllerMessage
{
    private readonly ILogger<ControllerMessages> _logger;
    private readonly ICommandManager _commandManager;
    private readonly IMessageHandler _handlerMessage;
    private readonly IPoolingCommands _poolingCommands;

    public Func<Exception, Exception> HandlerError { get; }
    public Action<object> HandlerMessage { get; }

    public ControllerMessages(
        ILogger<ControllerMessages> logger,
        ICommandManager commandManager,
        IMessageHandler handlerMessage,
        IPoolingCommands poolingCommands)
    {
        _logger = logger;
        _commandManager = commandManager;
        _handlerMessage = handlerMessage;
        _poolingCommands = poolingCommands;

        HandlerError = HandleExceptionImp;
        HandlerMessage = HandlerMessageImp;
    }

    public void HandlerMessageImp(object objectMessage)
    {
        _logger.LogInformation("receiving message");

        if (!_handlerMessage.IsValidMessage(objectMessage))
            throw new ArgumentException("Object Message not valid");

        var message = _handlerMessage.GetMessage(objectMessage);
        long chatId = _handlerMessage.GetChatId(objectMessage);

        _poolingCommands.TryGetPendingCommandResponse(chatId, out ICommand command);

        if (command is null && _commandManager.IsCommand(message))
        {
            command = _commandManager.GetCommand(message);
        }

        ExecuteCommand(objectMessage, chatId, command);
    }

    private void ExecuteCommand(
        object objectMessage, 
        long chatId, 
        ICommand command)
    {
        if (command is not null)
        {
            command.ExecuteAsync(objectMessage);

            _poolingCommands.TryAddCommand(
                chatId,
                _commandManager.GetNextCommand(command.NextStep));
        }
    }

    public Exception HandleExceptionImp(Exception exception)
    {
        _logger.LogError(exception, "Error in HandlerMessageTelegram");
        return exception;
    }

}
