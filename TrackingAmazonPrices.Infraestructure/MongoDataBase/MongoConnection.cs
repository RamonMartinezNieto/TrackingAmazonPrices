using MongoDB.Driver;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Domain.Configurations;

namespace TrackingAmazonPrices.Infraestructure.MongoDataBase;

public class MongoConnection
{
    private readonly ILogger<MongoConnection> _logger;
    private readonly IMongoClient _client;

    public MongoConnection(
        ILogger<MongoConnection> logger,
        IMongoClient client)
    {
        _logger = logger;
        _client = client;
    }

    private IMongoDatabase GetDataBase(string dataBase)
    {
        try
        {
            return _client.GetDatabase(dataBase);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception has ocurr {ErrMessage}", ex.Message);
        }
        return null;
    }
    
    public IMongoCollection<TDocument> GetCollection<TDocument>(string databaseName, string collectionName)
    {
        try
        {
            var database = GetDataBase(databaseName);
            return database.GetCollection<TDocument>(collectionName);
        }
        catch (Exception ex)
        {
            _logger.LogError("Imposible to get the collection {ErrMessage}", ex.Message);
        }
        return null;
    }
}