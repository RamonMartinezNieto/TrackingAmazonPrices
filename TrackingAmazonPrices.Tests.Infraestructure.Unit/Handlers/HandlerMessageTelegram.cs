namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Handlers;

public class HandlerMessageTelegram
{
    IBotClient<ITelegramBotClient> _botClient = Substitute.For<IBotClient<ITelegramBotClient>>();
    ILogger<HandlerMessageTelegram> _logger = Substitute.For<ILogger<HandlerMessageTelegram>>();

}
