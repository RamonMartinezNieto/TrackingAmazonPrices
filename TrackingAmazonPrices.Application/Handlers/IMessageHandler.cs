using TrackingAmazonPrices.Application.ApplicationFlow;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Application.Handlers;

public interface IMessageHandler
{
    bool IsValidMessage<TMessage>(TMessage typeMessage);

    bool IsCallBackQuery<TMessage>(TMessage typeMessage);

    MessageTypes GetTypeMessage(object typeMessage);

    Task<bool> SentMessage(object objectMessage, string textMessage);

    Task<bool> SentInlineKeyboardMessage(
        object objectMessage,
        string textMessage,
        object menu);

    string GetMessage<TMessage>(TMessage objectMessage);

    IControllerMessage SetControllerMessage(IControllerMessage controllerMessage);

    long GetChatId(object objectMessage);
}