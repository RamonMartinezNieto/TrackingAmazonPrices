using TrackingAmazonPrices.Application.Services;
using Telegram.Bot;
using TrackingAmazonPrices.Domain.Configurations;

namespace TrackingAmazonPrices.Infraestructure.Services;

public class BotClientTelegram : IBotClient<ITelegramBotClient>
{
    public ITelegramBotClient BotClient { get; }

    public BotClientTelegram(
        IOptions<BotConfig> botConfig)
    {
        BotClient = new TelegramBotClient(botConfig.Value.Token);
    }
}