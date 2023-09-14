using MongoDB.Driver;
using Telegram.Bot.Types;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Infraestructure.Mappers;
using TrackingAmazonPrices.Infraestructure.MongoDto;
using ZstdSharp.Unsafe;

namespace TrackingAmazonPrices.Infraestructure.MongoDataBase;

public class MongoUserService : IDatabaseUserHandler
{
    private readonly ILogger<MongoUserService> _logger;
    private readonly MongoConnection _connection;

    public MongoUserService(
        ILogger<MongoUserService> logger,
        MongoConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    public async Task<bool> SaveUserAsync(Domain.Entities.User user)
    {
        _logger.LogWarning("SAVE USER {Name}", user.Name);

        var collection = _connection.GetCollection<MongoUserDto>("test_database", "users");

        var mongoUser = user.ToMongoDto();

        var filter = FilterByUserId(mongoUser.UserId);

        var result = await collection.ReplaceOneAsync(filter, mongoUser, new ReplaceOptions { IsUpsert = true });

        return result.IsAcknowledged && (result.MatchedCount > 0 || result.UpsertedId != null);
    }

    private static FilterDefinition<MongoUserDto> FilterByUserId(long userId)
        => Builders<MongoUserDto>.Filter.Eq(x => x.UserId, userId);
}
