namespace TrackingAmazonPrices.Application.Services;

public interface IBotClient<TClient>
{
    public TClient BotClient { get; }
}