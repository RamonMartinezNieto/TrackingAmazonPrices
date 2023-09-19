using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Domain.Definitions;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class CommandManager : ICommandManager
{
    private readonly Dictionary<string, (Steps, Type)> _commands = new()
    {
        { CommandsMessages.START_COMMAND, (Steps.Start, typeof(StartCommand)) },
        { CommandsMessages.LANGUAGE_COMMAND, (Steps.Language, typeof(LanguageCommand)) },
        { CommandsMessages.DELETE_COMMAND, (Steps.Language, typeof(DeleteUserCommand)) },
    };

    private readonly IEnumerable<ICommand> _commandProvider;

    public CommandManager(
        IEnumerable<ICommand> commandProvider)
    {
        _commandProvider = commandProvider;
    }

    public ICommand GetCommand(string messageCommand)
    {
        if (_commands.TryGetValue(messageCommand, out var commandType))
        {
            Type type = commandType.Item2;
            return _commandProvider.FirstOrDefault(x => x.GetType() == type);
        }
        return NullCommand();
    }

    public bool IsCommand(string command)
        => _commands.ContainsKey(command);

    public ICommand NullCommand()
        => _commandProvider.FirstOrDefault(x => x.GetType() == typeof(NullCommand));
}