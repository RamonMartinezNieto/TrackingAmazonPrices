namespace TrackingAmazonPrices.Application.Callbacks;

public interface ICallback
{
    Task<bool> ExecuteAsync(object objectMessage, string dataCallback);
}