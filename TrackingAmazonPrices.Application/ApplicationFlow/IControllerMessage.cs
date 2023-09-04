
using TrackingAmazonPrices.Application.Handlers;

namespace TrackingAmazonPrices.Application.ApplicationFlow;

public interface IControllerMessage
{
    Func<Exception, Exception> HandlerError { get; }
    Action<object> HandlerMessage { get; }
}
