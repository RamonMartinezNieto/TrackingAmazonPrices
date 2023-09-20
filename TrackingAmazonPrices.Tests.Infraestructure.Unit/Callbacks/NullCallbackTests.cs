using TrackingAmazonPrices.Infraestructure.Callbacks;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Callbacks;

public class NullCallbackTests
{
    private readonly NullCallback _sut;

    public NullCallbackTests()
    {
        _sut = new NullCallback();
    }

    [Fact]
    public async Task ExecuteAsync_Always_ReturnsFalse()
    {
        var objectMessage = new object();
        var dataCallback = "someData";

        var result = await _sut.ExecuteAsync(objectMessage, dataCallback);

        result.Should().BeFalse();
    }
}