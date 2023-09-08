using TrackingAmazonPrices.Application.ApplicationFlow;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;

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

    public async void HandlerMessageImp(object objectMessage)
    {
        _logger.LogInformation("receiving message");

        if (_handlerMessage.IsCallBackQuery(objectMessage))
        {
            _logger.LogInformation("esto es un callback");
        }
        else if (_handlerMessage.IsValidMessage(objectMessage))
        {
            var message = _handlerMessage.GetMessage(objectMessage);
            long chatId = _handlerMessage.GetChatId(objectMessage);

            ICommand command = GetCommand(message, chatId);

            var (succes, nextCommand) = await TryExecuteCommand(command, objectMessage);

            if (succes)
            {
                _poolingCommands.TryAddCommand(chatId, nextCommand);
            }
        }
    }

    private ICommand GetCommand(string message, long chatId)
    {
        if (_poolingCommands.TryGetPendingCommandResponse(chatId, out ICommand command))
            return command;

        if (_commandManager.IsCommand(message))
        {
            return _commandManager.GetCommand(message);
        }

        return _commandManager.NullCommand();
    }

    private async Task<(bool succes, ICommand nextCommand)> TryExecuteCommand(
        ICommand command,
        object objectMessage)
    {
        if (await command.ExecuteAsync(objectMessage))
        {
            var nextCommand = _commandManager.GetNextCommand(command.NextStep);
            return (true, nextCommand);
        }
        return (true, _commandManager.NullCommand());
    }

    public Exception HandleExceptionImp(Exception exception)
    {
        _logger.LogError(exception, "Error in HandlerMessageTelegram");
        return exception;
    }
}