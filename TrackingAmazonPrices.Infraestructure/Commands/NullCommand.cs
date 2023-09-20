using TrackingAmazonPrices.Application.Command;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class NullCommand : ICommand
{
    public Task<bool> ExecuteAsync(object objectMessage)
        => Task.FromResult(false);
}