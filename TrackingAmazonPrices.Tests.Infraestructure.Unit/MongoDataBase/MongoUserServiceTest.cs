using MongoDB.Bson;
using MongoDB.Driver;
using TrackingAmazonPrices.Infraestructure.MongoDto;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.MongoDataBase;

public class MongoUserServiceTest
{

    private readonly IDatabaseUserService _sut; 

    public MongoUserServiceTest()
    {

        _sut = Substitute.For<IDatabaseUserService>();
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
