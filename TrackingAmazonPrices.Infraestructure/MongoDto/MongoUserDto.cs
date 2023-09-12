using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Infraestructure.MongoDto;

public class MongoUserDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("name")]
    public string Name { get; set; }
    [BsonElement("userId")]
    public long UserId { get; set; }
    [BsonElement("chatId")]
    public long ChatId { get; set; }
    [BsonElement("platform")]
    public PlatformType Platform { get; set; }  
    
    [BsonElement("language")]
    [BsonIgnoreIfNull]
    public Language Language { get; set; }
}
