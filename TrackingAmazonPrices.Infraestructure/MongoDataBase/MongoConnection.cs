using MongoDB.Driver;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Domain.Configurations;

namespace TrackingAmazonPrices.Infraestructure.MongoDataBase;

public sealed class MongoConnection
{
    private readonly ILogger<MongoConnection> _logger;
    private readonly DatabaseConfig _databaseConfig;
    private readonly IDatabaseHandler _databaseHandler;
    private readonly MongoClient _client;

    public MongoConnection(
        ILogger<MongoConnection> logger,
        IOptions<DatabaseConfig> databaseConfig,
        IDatabaseHandler databaseHandler)
    {
        _logger = logger;
        _databaseConfig = databaseConfig.Value;
        _databaseHandler = databaseHandler;

        _client = GetClient();
    }

    private IMongoDatabase GetDataBase(string dataBase)
    {
        try
        {
            return _client.GetDatabase(dataBase);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An exception has ocurr {ex.Message}");
        }
        return null;
    }
    
    public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName)
    {
        try
        {
            var database = GetDataBase(_databaseConfig.Database);
            return database.GetCollection<TDocument>(collectionName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Imposible to get the collection " + ex.Message);
        }
        return null;
    }

    private MongoClient GetClient()
    {
        try
        {
            return new MongoClient(_databaseHandler.GetConnectionString());
        }
        catch (MongoConnectionException ex)
        {
            _logger.LogError($"An exception has ocurr {ex.Message}");
            throw new MongoConnectionException(ex.ConnectionId, "MongoConnectionGetClient : Is not possible to connect mongo");
        }
        catch (Exception ex)
        {
            throw new Exception("MongoConnectionGetClient : Is not possible to connect mongo", ex);
        }
    }
}