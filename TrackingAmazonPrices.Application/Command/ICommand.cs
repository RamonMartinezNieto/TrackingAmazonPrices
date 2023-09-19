namespace TrackingAmazonPrices.Application.Command;

public interface ICommand
{
    Task<bool> ExecuteAsync(object objectMessage);
}