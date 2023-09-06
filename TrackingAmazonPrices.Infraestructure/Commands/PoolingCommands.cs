using System.Collections.Concurrent;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class PoolingCommands : IPoolingCommands
{
    private readonly ILogger<PoolingCommands> _logger;

    public static ConcurrentDictionary<long, ICommand> PoolingTestCommands { get; set; }

    public PoolingCommands(ILogger<PoolingCommands> logger)
    {
        PoolingTestCommands = new();
        _logger = logger;
    }


    public bool TryAddCommand(long chatId, ICommand command)
    {
        if (command is not null)
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
