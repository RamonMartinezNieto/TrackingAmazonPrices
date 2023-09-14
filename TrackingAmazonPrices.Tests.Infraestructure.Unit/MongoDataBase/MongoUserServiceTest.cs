using MongoDB.Bson;
using MongoDB.Driver;
using TrackingAmazonPrices.Infraestructure.MongoDataBase;
using TrackingAmazonPrices.Infraestructure.MongoDto;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.MongoDataBase;

public class MongoUserServiceTest
{
    private readonly ILogger<MongoUserService> _logger = Substitute.For<ILogger<MongoUserService>>();
    private readonly IMongoClient _client = Substitute.For<IMongoClient>();

    private readonly IDatabaseUserService _sut;

    public MongoUserServiceTest()
    {
        _sut = new MongoUserService(_logger, _client);
    }

    [Fact]
    public async Task SaveUserAsync_ReturnTure_WhenUserIsValid()
    {
        var user = GetUser();

        IMongoDatabase dataBase = Substitute.For<IMongoDatabase>();
        IMongoCollection<MongoUserDto> collection = Substitute.For<IMongoCollection<MongoUserDto>>();
        ReplaceOneResult replaceResult = Substitute.For<ReplaceOneResult>();
        var upsertedId = new BsonObjectId(ObjectId.GenerateNewId());

        _client.GetDatabase(Arg.Any<string>()).Returns(dataBase);
        dataBase.GetCollection<MongoUserDto>(Arg.Any<string>()).Returns(collection);

        collection.ReplaceOneAsync(
            Arg.Any<FilterDefinition<MongoUserDto>>(),
            Arg.Any<MongoUserDto>(),
            Arg.Any<ReplaceOptions>()).Returns(replaceResult);

        replaceResult.IsAcknowledged.Returns(true);
        replaceResult.UpsertedId.Returns((BsonValue)upsertedId);

        var result = await _sut.SaveUserAsync(user);

        result.Should().BeTrue();
    }

    private static Domain.Entities.User GetUser()
    {
        return new()
        {
            Name = "Pepe",
            Platform = PlatformType.Telegram,
            UserId = 2222,
        };
    }
}