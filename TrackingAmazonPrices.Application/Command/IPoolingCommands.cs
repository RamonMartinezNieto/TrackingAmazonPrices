using System.Collections.Concurrent;

namespace TrackingAmazonPrices.Application.Command;

public interface IPoolingCommands
{
    static ConcurrentDictionary<long, ICommand> PoolingTestCommands { get; }

    bool TryGetPendingCommandResponse(long chatId, out ICommand command);

    bool TryAddCommand(long chatId, ICommand command);

}
