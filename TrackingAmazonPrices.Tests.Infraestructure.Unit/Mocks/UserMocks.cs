namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Mocks;

public static class UserMocks
{
    public static Domain.Entities.User GetUserWithLanguage()
        => new() { Language = LanguageType.English };
}
