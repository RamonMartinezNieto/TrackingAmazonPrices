using FluentAssertions;
using TrackingAmazonPrices.Domain.Entities;
using TrackingAmazonPrices.Domain.Enums;
using Xunit;

namespace TrackingAmazonPrices.Tests.Domain.Unit.Entities;

public class UserTests
{
    //public string Name { get; init; }
    //public long UserId { get; init; }
    //public PlatformType Platform { get; init; }
    //public Language Language { get; init; }

    [Fact]
    public void InstantiateUser_ReturnUsing_VoidConstructor() 
    {

        var user = new User();

        user.Language.LanguageCode.Should().Be(LanguageType.English);
        user.Name.Should().BeNull();
        user.UserId.Should().Be(default);
        user.Platform.Should().Be(default);
    }


    [Fact]
    public void InstantiateUser_ReturnUsing_ConstructorWithoutLanguage() 
    {

        var user = new User("ramon", 123L, PlatformType.Telegram);

        user.Language.LanguageCode.Should().Be(LanguageType.English);
        user.Name.Should().Be("ramon");
        user.UserId.Should().Be(123L);
        user.Platform.Should().Be(PlatformType.Telegram);
    }

    [Fact]
    public void InstantiateUser_ReturnUsing_ConstructorWithLanguage() 
    {

        var user = new User("ramon", 123L, PlatformType.Telegram, new Language("es"));

        user.Language.LanguageCode.Should().Be(LanguageType.Spanish);
        user.Name.Should().Be("ramon");
        user.UserId.Should().Be(123L);
        user.Platform.Should().Be(PlatformType.Telegram);
    }
}
