using MongoDB.Driver;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Enums;
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

        var collection = GetUserCollection();
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

        var collection = GetUserCollection();

        var filter = FilterByUserId(id);

        var userLanguage = collection.Find(filter).FirstOrDefault();

        if (userLanguage is null)
            return Task.FromResult(LanguageType.Default);

        return Task.FromResult(userLanguage.Language);
    }
    
    public async Task<bool> DeleteUser(long id)
    {
        _logger.LogInformation("Deleting user {id}", id);

        var collection = GetUserCollection();

        var filter = FilterByUserId(id); 

        var deleteResult = await collection.DeleteOneAsync(filter);

        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;

    }

    public async Task<bool> UserExists(long id)
    {
        var collection = GetUserCollection();

        var filter = FilterByUserId(id);

        var user = await collection.Find(filter).FirstOrDefaultAsync();

        return user != null;
    }

    private static FilterDefinition<MongoUserDto> FilterByUserId(long id)
        => Builders<MongoUserDto>.Filter.Eq(x => x.UserId, id);

    private IMongoCollection<MongoUserDto> GetUserCollection()
    {
        try
        {
            var database = _client.GetDatabase(USER_DATABASE);
            return database.GetCollection<MongoUserDto>(USER_COLLECTION);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has ocurr {ErrMessage}", ex.Message);
            throw new MongoException("MongoUserService => GetDataBase => Some problem GettingDatabase", ex);
        }
    }

}