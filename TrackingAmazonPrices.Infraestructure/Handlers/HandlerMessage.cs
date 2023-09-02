using TrackingAmazonPrices.Application.Handlers;

namespace TrackingAmazonPrices.Infraestructure.Handlers;

public abstract class HandlerMessage : IHandlerMessage
{
    internal Func<Exception, Exception> _handlerError;
    internal Action<object> _handlerMessage;

    public HandlerMessage(
        Func<Exception, Exception> handlerError,
        Action<object> handlerMessage)
    {
        _handlerError = handlerError;
        _handlerMessage = handlerMessage;
    }

    public abstract bool IsValidMessage<TMessage>(TMessage message);

    public abstract void PrintMessage(object objectMessage);

    public abstract Task SentMessage(object chatId, string v, CancellationToken cts);
}