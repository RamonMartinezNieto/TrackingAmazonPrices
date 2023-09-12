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
        result.ChatId.Should().Be(user.ChatId);
        result.UserId.Should().Be(user.UserId);
        result.Name.Should().Be(user.Name);
        result.Platform.Should().Be(user.Platform);
        result.Language.Should().Be(Language.English);
    }


    [Fact]
    public void ToMongoDto_ReturnCompleteUser_WhenAllDataIsValid() 
    {
        Domain.Entities.User user = GetUser();
        user.Language = Language.Spanish;

        var result = user.ToMongoDto();

        result.Should().BeAssignableTo<MongoUserDto>();
        result.ChatId.Should().Be(user.ChatId);
        result.UserId.Should().Be(user.UserId);
        result.Name.Should().Be(user.Name);
        result.Platform.Should().Be(user.Platform);
        result.Language.Should().Be(Language.Spanish);
    }


    private static Domain.Entities.User GetUser()
    {
        return new()
        {
            ChatId = 1111,
            Name = "Pepe",
            Platform = PlatformType.Telegram,
            UserId = 2222,
        };
    }
}
