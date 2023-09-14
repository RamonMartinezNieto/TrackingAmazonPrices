using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Domain.Configurations;

namespace TrackingAmazonPrices.Infraestructure.MongoDataBase;

public class MongoDatabaseHandler : IDatabaseHandler
{
    private readonly DatabaseConfig _config;

    public MongoDatabaseHandler(
        IOptions<DatabaseConfig> config)
    {
        _config = config.Value;
    }


    public string GetConnectionString()
    {
        return $"{_config.Protocol}://{_config.User}:{_config.Password}@{_config.Host}/";
    }
}
