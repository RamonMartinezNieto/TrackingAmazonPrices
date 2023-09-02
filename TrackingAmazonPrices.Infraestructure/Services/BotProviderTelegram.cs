using Microsoft.Extensions.Options;
using Telegram.Bot;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Configurations;

namespace TrackingAmazonPrices.Infraestructure.Services;

public class BotProviderTelegram : IBotProvider
{
    public TelegramBotClient BotCLient { get; }

    public BotProviderTelegram(IOptions<BotConfig> options)
    {
        BotCLient = new TelegramBotClient(options.Value.Token);
    }

    public object GetBotClient() => BotCLient;
}