using Telegram.Bot.Types;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Mocks;

public static class MockMessages
{
    public static Update GetMockMessage()
    {
        Update message = new()
        {
            Message = new()
        };
        message.Message.Text = "Some text message";
        message.Message.Chat = new()
        {
            Id = 123L
        };
        message.Message.From = new()
        {
            Id = 123L,
            Username = "test"
        };

        return message;
    }

    public static Update GetMockCallback() => new()
    {
        CallbackQuery = new()
        {
            Data = "some_callback_data"
        }
    };
}