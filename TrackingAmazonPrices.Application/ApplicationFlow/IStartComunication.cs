using TrackingAmazonPrices.Application.Handlers;

namespace TrackingAmazonPrices.Application.ApplicationFlow;

public interface IStartComunication
{
    public IHandlerMessage Start<THandlerMessage>(
        Func<Exception, Exception> handlerError,
        Action<object> handlerMessage)
        where THandlerMessage : IHandlerMessage;
}