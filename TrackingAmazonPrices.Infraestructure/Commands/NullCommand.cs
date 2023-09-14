using TrackingAmazonPrices.Application.Command;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class NullCommand : ICommand
{
    public Steps NextStep => Steps.Nothing;

    public Task<bool> ExecuteAsync(object objectMessage)
    {
        return Task.FromResult(false);
    }
}