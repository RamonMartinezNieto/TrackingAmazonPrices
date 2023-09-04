using System.Collections.Generic;
using System.Linq;
using TrackingAmazonPrices.Application.Command;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class CommandManager : ICommandManager
{
    private static readonly List<string> _commands = new()
    {
        "/start",
        "/test"
    };

    private IEnumerable<ICommand> _commandProvider;

    public CommandManager(
        IEnumerable<ICommand> commandProvider)
    {
        _commandProvider = commandProvider;
    }

    //TODO Change with a factory or other
    public ICommand GetCommand(string messageCommand)
        => messageCommand switch
        {
            "/start" => _commandProvider.FirstOrDefault(x => x.GetType() == typeof(StartCommand)),
            "/test" => _commandProvider.FirstOrDefault(x => x.GetType() == typeof(TestCommand)),
            _ => null,
        };

    public bool IsCommand(string messageCommand)
        => _commands.Contains(messageCommand);
}
