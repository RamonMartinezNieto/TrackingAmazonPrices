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
            Platform = user.Platform,
            Language = user.Language.LanguageCode
        };
    }

    public static User ToUser(this MongoUserDto mongoUserDto)
    {
        return new User
        {
            Name = mongoUserDto.Name,
            UserId = mongoUserDto.UserId,
            Platform = mongoUserDto.Platform,
            Language = new Language(mongoUserDto.Language)
        };
    }
}