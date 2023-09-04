using System.Collections.Generic;
using System.Linq;
using TrackingAmazonPrices.Application.Command;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class CommandManager : ICommandManager
{
    private readonly Dictionary<string, Type> _commands = new()
    {
        { "/start", typeof(StartCommand) },
        { "/test", typeof(TestCommand) },
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
            return _commandProvider.FirstOrDefault(x => x.GetType() == commandType);

        return null;
    }

    public bool IsCommand(string messageCommand)
        => _commands.ContainsKey(messageCommand);
}
