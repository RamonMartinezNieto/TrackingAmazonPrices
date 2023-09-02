using TrackingAmazonPrices.Application.Services;
using Telegram.Bot;

namespace TrackingAmazonPrices.Infraestructure.Services;

public class BotClientTelegram : IBotClient
{
    public object BotClient { get; set; }

    public BotClientTelegram(IBotProvider clientProvider)
    {
        BotClient = (ITelegramBotClient)clientProvider.GetBotClient();
    }
}