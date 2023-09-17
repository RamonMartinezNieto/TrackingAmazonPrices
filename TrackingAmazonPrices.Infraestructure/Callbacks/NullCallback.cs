using TrackingAmazonPrices.Application.Callbacks;

namespace TrackingAmazonPrices.Infraestructure.Callbacks;

internal class NullCallback : ICallback
{
    public Task<bool> ExecuteAsync(object objectMessage, string data)
        => Task.FromResult(false);
}