using Telegram.Bot.Types;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Commands;

public class NullCommandTest
{
    [Fact]
    public void InstantiateNullCommand_NextStep_ShouldBeNothing()
    {
        NullCommand startCommand = new();
        startCommand.NextStep.Should().Be(Steps.Nothing);
    }

    [Fact]
    public async void Execute_ShouldReturnFalse()
    {
        NullCommand startCommand = new();
        var result = await startCommand.ExecuteAsync(new object());
        startCommand.NextStep.Should().Be(Steps.Nothing);
        result.Should().BeFalse();
    }
}
