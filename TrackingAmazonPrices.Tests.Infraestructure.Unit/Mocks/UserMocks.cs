using TrackingAmazonPrices.Domain.Entities;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Mocks;

public static class UserMocks
{
    public static User GetUser()
        => new()
        {
            Name = "Pepe",
            Platform = PlatformType.Telegram,
            Language = LanguageType.Spanish,
            UserId = 2222,
        };

    public static User GetUserDefaultLang()
        => new()
        {
            Name = "Pepe",
            Platform = PlatformType.Telegram,
            UserId = 2222,
        };

    public static User GetUserWithLanguage()
        => new() { Language = LanguageType.English };
}