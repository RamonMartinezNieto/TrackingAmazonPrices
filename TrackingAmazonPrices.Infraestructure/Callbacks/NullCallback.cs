using TrackingAmazonPrices.Application.Callbacks;

namespace TrackingAmazonPrices.Infraestructure.Callbacks;

public class NullCallback : ICallback
{
    public Task<bool> ExecuteAsync(object objectMessage, string dataCallback)
        => Task.FromResult(false);
}