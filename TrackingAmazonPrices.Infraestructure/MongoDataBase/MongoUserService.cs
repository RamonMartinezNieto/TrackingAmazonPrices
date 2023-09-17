using MongoDB.Driver;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain;
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
        _logger.LogInformation("SAVE USER {Name}", user.Name);

        var collection = GetUserCollertion();
        var filter = FilterByUserId(user.UserId);

        var result = await collection.ReplaceOneAsync(
            filter,
            user.ToMongoDto(),
            new ReplaceOptions { IsUpsert = true });

        return result.IsAcknowledged && (result.MatchedCount > 0 || result.UpsertedId != null);
    }

    public Task<LanguageType> GetLanguage(long id)
    {
        _logger.LogInformation("Getting user language {id}", id);

        var collection = GetUserCollertion();

        var filter = Builders<MongoUserDto>.Filter.Eq(x => x.UserId, id);

        var userLanguage = collection.Find(filter).FirstOrDefault();

        if (userLanguage is null)
            return Task.FromResult(LanguageType.Default);

        return Task.FromResult(userLanguage.Language);
    }

    private static FilterDefinition<MongoUserDto> FilterByUserId(long userId)
        => Builders<MongoUserDto>.Filter.Eq(x => x.UserId, userId);

    private IMongoCollection<MongoUserDto> GetUserCollertion()
    {
        var database = GetDataBase();
        return database.GetCollection<MongoUserDto>(USER_COLLECTION);
    }

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