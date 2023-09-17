using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Domain.Entities;

public class User
{
    public string Name { get; init; }
    public long UserId { get; init; }
    public PlatformType Platform { get; init; }
    public Language Language { get; set; }

    public User()
    {
        Language = new();
    }

    public User(
        string name,
        long userId,
        PlatformType platform) : this()
    {
        Name = name;
        UserId = userId;
        Platform = platform;
    }

    public User(
        string name,
        long userId,
        PlatformType platform,
        Language language)
    {
        Name = name;
        UserId = userId;
        Platform = platform;
        Language = language;
    }
}