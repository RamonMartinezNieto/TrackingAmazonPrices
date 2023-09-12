using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Domain.Entities;

public class User
{
    public string Name { get; set; }
    public long UserId { get; set; }
    public long ChatId { get; set; }
    public PlatformType Platform { get; set; }
    public Language Language { get; set; } = Language.English;
}
