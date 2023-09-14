using TrackingAmazonPrices.Domain.Entities;
using TrackingAmazonPrices.Infraestructure.Mappers;
using TrackingAmazonPrices.Infraestructure.MongoDto;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Mappers;

public class MongoMappersTests
{
    [Fact]
    public void ToMongoDto_ReturnCompleteUser_WhenAllDataIsValid_DefaultLanguage()
    {
        Domain.Entities.User user = GetUser();

        var result = user.ToMongoDto();

        result.Should().BeAssignableTo<MongoUserDto>();
        result.UserId.Should().Be(user.UserId);
        result.Name.Should().Be(user.Name);
        result.Platform.Should().Be(user.Platform);
        result.Language.Should().Be(LanguageType.English);
    }

    [Fact]
    public void ToMongoDto_ReturnCompleteUser_WhenAllDataIsValid()
    {
        Domain.Entities.User user = GetUserWithLanguage();

        var result = user.ToMongoDto();

        result.Should().BeAssignableTo<MongoUserDto>();
        result.UserId.Should().Be(user.UserId);
        result.Name.Should().Be(user.Name);
        result.Platform.Should().Be(user.Platform);
        result.Language.Should().Be(LanguageType.Spanish);
    }

    private static Domain.Entities.User GetUser()
    {
        return new()
        {
            Name = "Pepe",
            Platform = PlatformType.Telegram,
            UserId = 2222,
        };
    }

    private static Domain.Entities.User GetUserWithLanguage()
    {
        return new()
        {
            Name = "Pepe",
            Platform = PlatformType.Telegram,
            UserId = 2222,
            Language = new Language("es")
        };
    }
}