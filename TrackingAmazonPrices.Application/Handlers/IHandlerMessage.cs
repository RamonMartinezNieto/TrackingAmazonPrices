namespace TrackingAmazonPrices.Application.Handlers;

public interface IHandlerMessage
{
    abstract bool IsValidMessage<TMessage>(TMessage message);

    abstract Task SentMessage(object chatId, string message, CancellationToken cts);

    abstract void PrintMessage(object objectMessage);
}