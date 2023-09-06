using TrackingAmazonPrices.Application.ApplicationFlow;

namespace TrackingAmazonPrices.Application.Handlers;

public interface IMessageHandler
{
    bool IsValidMessage<TMessage>(TMessage message);

    Task SentMessage(object chatId, string message);

    string GetMessage<TMessage>(TMessage objectMessage);

    void SetControllerMessage(IControllerMessage controllerMessage);

    long GetChatId(object objectMessage);
}