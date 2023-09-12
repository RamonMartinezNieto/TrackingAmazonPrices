using TrackingAmazonPrices.Domain.Entities;
using TrackingAmazonPrices.Infraestructure.MongoDto;

namespace TrackingAmazonPrices.Infraestructure.Mappers;

public static class MongoMapper
{
    public static MongoUserDto ToMongoDto(this User user) 
    {
        return new MongoUserDto
        {
            Name = user.Name,
            UserId = user.UserId,
            ChatId = user.ChatId,
            Platform = user.Platform,
            Language = user.Language
        };
    }

    public static User ToUser(this MongoUserDto mongoUserDto)
    {
        return new User
        {
            Name = mongoUserDto.Name,
            UserId = mongoUserDto.UserId,
            ChatId = mongoUserDto.ChatId,
            Platform = mongoUserDto.Platform,
            Language = mongoUserDto.Language
        };
    }
}
