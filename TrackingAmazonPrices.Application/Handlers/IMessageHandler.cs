using TrackingAmazonPrices.Application.ApplicationFlow;

namespace TrackingAmazonPrices.Application.Handlers;

public interface IMessageHandler
{
    bool IsValidMessage<TMessage>(TMessage typeMessage);

    bool IsCallBackQuery<TMessage>(TMessage typeMessage);

    MessageTypes GetTypeMessage(object objectMessage);

    Task<bool> SentMessageAsync(object objectMessage, string textMessage);

    Task<bool> AnswerdCallback(object objectMessage, string textMessage);

    Task<bool> SentInlineKeyboardMessage(
        object objectMessage,
        string textMessage,
        object menu);

    string GetMessage<TMessage>(TMessage objectMessage);

    string GetCallbackMessage<TMessage>(TMessage objectMessage);

    Task<User> GetUser(object objectMessage);

    IControllerMessage SetControllerMessage(IControllerMessage controllerMessage);

    long GetChatId(object objectMessage);
}