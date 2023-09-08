namespace TrackingAmazonPrices.Application.Services;

public interface IBotClient<out TClient>
{
    public TClient BotClient { get; }
}