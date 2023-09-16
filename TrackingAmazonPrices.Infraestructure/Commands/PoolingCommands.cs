using System.Collections.Concurrent;
using TrackingAmazonPrices.Application.Command;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class PoolingCommands : IPoolingCommands
{
    public static ConcurrentDictionary<long, ICommand> PoolingTestCommands { get; private set; }

    public PoolingCommands()
    {
        PoolingTestCommands = new();
    }

    public bool TryAddCommand(long chatId, ICommand command)
    {
        if (command is not null && command is not NullCommand)
        {
            PoolingTestCommands.TryAdd(chatId, command);
            return true;
        }
        return false;
    }

    public bool TryGetPendingCommandResponse(long chatId, out ICommand command)
    {
        if (PoolingTestCommands.TryRemove(chatId, out command))
        {
            return true;
        }
        return false;
    }
}