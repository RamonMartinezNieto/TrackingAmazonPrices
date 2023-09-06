using System.Collections.Generic;
using System.Linq;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class CommandManager : ICommandManager
{
    private readonly Dictionary<string, (Steps,Type)> _commands = new()
    {
        { "/start", (Steps.Start, typeof(StartCommand)) },
        { "/test", (Steps.Test, typeof(TestCommand)) },
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

        return null;
    }

    public ICommand GetNextCommand(Steps? step)
    {
        if (step.HasValue)
        {
            var matchingEntry = _commands.FirstOrDefault(entry => entry.Value.Item1 == step.Value);

            if (!matchingEntry.Equals(default(KeyValuePair<string, (Steps, Type)>)))
            {
                Type type = matchingEntry.Value.Item2;
                return _commandProvider.FirstOrDefault(x => x.GetType() == type);
            }
        }

        return null;
    }


    public bool IsCommand(string messageCommand)
        => _commands.ContainsKey(messageCommand);

}
