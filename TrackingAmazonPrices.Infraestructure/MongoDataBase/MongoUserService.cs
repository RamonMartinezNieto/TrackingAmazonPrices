using MongoDB.Driver;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Infraestructure.Mappers;
using TrackingAmazonPrices.Infraestructure.MongoDto;

namespace TrackingAmazonPrices.Infraestructure.MongoDataBase;

public class MongoUserService : IDatabaseUserService
{
    private const string USER_DATABASE = "test_database";
    private const string USER_COLLECTION = "users";

    private readonly ILogger<MongoUserService> _logger;
    private readonly IMongoClient _client;

    public MongoUserService(
        ILogger<MongoUserService> logger,
        IMongoClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task<bool> SaveUserAsync(Domain.Entities.User user)
    {
        _logger.LogWarning("SAVE USER {Name}", user.Name);

        var database = GetDataBase();
        var collection = database.GetCollection<MongoUserDto>(USER_COLLECTION);
        var mongoUser = user.ToMongoDto();

        var filter = FilterByUserId(mongoUser.UserId);

        var result = await collection.ReplaceOneAsync(filter, mongoUser, new ReplaceOptions { IsUpsert = true });

        return result.IsAcknowledged && (result.MatchedCount > 0 || result.UpsertedId != null);
    }

    private static FilterDefinition<MongoUserDto> FilterByUserId(long userId)
        => Builders<MongoUserDto>.Filter.Eq(x => x.UserId, userId);

    private IMongoDatabase GetDataBase()
    {
        try
        {
            return _client.GetDatabase(USER_DATABASE);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has ocurr {ErrMessage}", ex.Message);
            throw new MongoException("MongoUserService => GetDataBase => Some problem GettingDatabase", ex);
        }
    }
}