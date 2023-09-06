using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Application.Command;

public interface ICommand
{
    public Steps NextStep { get; }
    Task ExecuteAsync(object objectMessage);
}
