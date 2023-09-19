using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Infraestructure.Commands;

public class NullCommand : ICommand
{
    public Task<bool> ExecuteAsync(object objectMessage)
        => Task.FromResult(false);
}