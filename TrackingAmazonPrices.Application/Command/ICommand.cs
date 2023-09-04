namespace TrackingAmazonPrices.Application.Command;

public interface ICommand
{
    Task ExecuteAsync(object objectMessage, CancellationToken cancellationToken);
}
