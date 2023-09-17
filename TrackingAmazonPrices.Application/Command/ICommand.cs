namespace TrackingAmazonPrices.Application.Command;

public interface ICommand
{
    public Steps NextStep { get; }

    Task<bool> ExecuteAsync(object objectMessage);
}