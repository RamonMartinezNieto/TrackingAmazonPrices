using TrackingAmazonPrices.Application.ApplicationFlow;

namespace TrackingAmazonPrices.Application.Handlers;

public interface IMessageHandler
{
    bool IsValidMessage<TMessage>(TMessage message);

    Task SentMessage(object chatId, string message, CancellationToken cts);

    void PrintMessage(object objectMessage);

    string GetMessage<TMessage>(TMessage objectMessage);

    void SetControllerMessage(IControllerMessage controllerMessage);
}