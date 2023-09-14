using TrackingAmazonPrices.Application.ApplicationFlow;

namespace TrackingAmazonPrices.Application.Handlers;

public interface IMessageHandler
{
    bool IsValidMessage<TMessage>(TMessage typeMessage);

    bool IsCallBackQuery<TMessage>(TMessage typeMessage);

    MessageTypes GetTypeMessage(object objectMessage);

    Task<bool> SentMessage(object objectMessage, string textMessage);

    Task<bool> SentInlineKeyboardMessage(
        object objectMessage,
        string textMessage,
        object menu);

    string GetMessage<TMessage>(TMessage objectMessage);

    User GetUser(object objectMessage);

    IControllerMessage SetControllerMessage(IControllerMessage controllerMessage);

    long GetChatId(object objectMessage);
}