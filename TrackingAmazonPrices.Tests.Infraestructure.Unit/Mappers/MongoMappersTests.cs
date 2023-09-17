﻿using TrackingAmazonPrices.Domain;
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


    [Fact]
    public void ToUser_ReturnCompleteUser_WhenAllDataIsValid()
    {
        MongoUserDto mongoUser = new() 
        {
            Id = "123",
            Name = "Test",
            UserId = 123L,
            Platform = 0,
            Language = LanguageType.Spanish,
        };

        var result = mongoUser.ToUser();

        result.UserId.Should().Be(123L);
        result.Name.Should().Be("Test");
        result.Platform.Should().Be(PlatformType.Telegram);
        result.Language.LanguageCode.Should().Be(LanguageType.Spanish);
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