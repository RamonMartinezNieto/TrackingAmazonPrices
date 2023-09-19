namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Commands;

public class NullCommandTest
{
    [Fact]
    public async void Execute_ShouldReturnFalse()
    {
        NullCommand startCommand = new();
        var result = await startCommand.ExecuteAsync(new object());
        result.Should().BeFalse();
    }
}