using MongoDB.Bson;
using MongoDB.Driver;
using NSubstitute;
using NSubstitute.Core.Arguments;
using TrackingAmazonPrices.Infraestructure.MongoDataBase;
using TrackingAmazonPrices.Infraestructure.MongoDto;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.MongoDataBase;

public class MongoUserServiceTest
{

    private readonly ILogger<MongoUserService> _logger = Substitute.For<ILogger<MongoUserService>>();
    private readonly MongoConnection _mongoConnection;
    private readonly MongoUserService _sut; 

    public MongoUserServiceTest()
    {

        _sut = Substitute.ForPartsOf<MongoUserService>(_logger, _mongoConnection);
    }


    [Fact]
    public async Task SaveUserAsync_ReturnTure_WhenUserIsValid() 
    {
        var user = GetUser();

        var mongoCollection = Substitute.For<IMongoCollection<MongoUserDto>>();
        var replaceOneCol = Substitute.For<ReplaceOneResult>();

        
        mongoCollection.ReplaceOneAsync(
            Arg.Any<FilterDefinition<MongoUserDto>>(),
            Arg.Any<MongoUserDto>(),
            Arg.Any<ReplaceOptions>())
            .Returns(replaceOneCol);

        replaceOneCol.IsAcknowledged.Returns(true);
        replaceOneCol.MatchedCount.Returns(0);
        replaceOneCol.UpsertedId.Returns(Arg.Any<BsonValue>());

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
