using MongoDB.Bson;
using MongoDB.Driver;
using NSubstitute.Core;
using TrackingAmazonPrices.Infraestructure.Mappers;
using TrackingAmazonPrices.Infraestructure.MongoDataBase;
using TrackingAmazonPrices.Infraestructure.MongoDto;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.MongoDataBase;

public class MongoUserServiceTest
{
    private readonly ILogger<MongoUserService> _logger = Substitute.For<ILogger<MongoUserService>>();
    private readonly IMongoClient _client = Substitute.For<IMongoClient>();
    private readonly IMongoDatabase _dataBase = Substitute.For<IMongoDatabase>();
    private readonly IMongoCollection<MongoUserDto> _collection = Substitute.For<IMongoCollection<MongoUserDto>>();

    private readonly IDatabaseUserService _sut;

    public MongoUserServiceTest()
    {
        _sut = new MongoUserService(_logger, _client);

        _client.GetDatabase(Arg.Any<string>()).Returns(_dataBase);
        _dataBase.GetCollection<MongoUserDto>(Arg.Any<string>()).Returns(_collection);

    }

    [Fact]
    public async Task SaveUserAsync_ReturnTrue_WhenUserIsValid()
    {
        var user = GetUser();

        ReplaceOneResult replaceResult = Substitute.For<ReplaceOneResult>();
        var upsertedId = new BsonObjectId(ObjectId.GenerateNewId());

        _collection.ReplaceOneAsync(
            Arg.Any<FilterDefinition<MongoUserDto>>(),
            Arg.Any<MongoUserDto>(),
            Arg.Any<ReplaceOptions>()).Returns(replaceResult);

        replaceResult.IsAcknowledged.Returns(true);
        replaceResult.UpsertedId.Returns((BsonValue)upsertedId);

        var result = await _sut.SaveUserAsync(user);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteUser_True_WhenUserIsDeleted() 
    {
        var user = GetUser();

        DeleteResult deleteResult = Substitute.For<DeleteResult>();

        _collection.DeleteOneAsync(
            Arg.Any<FilterDefinition<MongoUserDto>>()
            ).Returns(deleteResult);

        deleteResult.IsAcknowledged.Returns(true);
        deleteResult.DeletedCount.Returns(1);

        var result = await _sut.DeleteUser(user.UserId);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteUser_False_WhenUserNotDeleted() 
    {
        var user = GetUser();

        DeleteResult deleteResult = Substitute.For<DeleteResult>();

        _collection.DeleteOneAsync(
            Arg.Any<FilterDefinition<MongoUserDto>>()
            ).Returns(deleteResult);

        deleteResult.IsAcknowledged.Returns(true);
        deleteResult.DeletedCount.Returns(0);

        var result = await _sut.DeleteUser(user.UserId);

        result.Should().BeFalse();
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