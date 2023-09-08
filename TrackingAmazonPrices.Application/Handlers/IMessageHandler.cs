using TrackingAmazonPrices.Application.ApplicationFlow;

namespace TrackingAmazonPrices.Application.Handlers;

public interface IMessageHandler
{
    bool IsValidMessage<TMessage>(TMessage typeMessage);

    bool IsCallBackQuery<TMessage>(TMessage typeMessage);

    Task<bool> SentMessage(object objectMessage, string textMessage);

    string GetMessage<TMessage>(TMessage objectMessage);

    void SetControllerMessage(IControllerMessage controllerMessage);

    long GetChatId(object objectMessage);
}