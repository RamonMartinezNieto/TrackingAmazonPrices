using Telegram.Bot.Types;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Commands;

public class StartCommandTests
{
    private readonly IMessageHandler _messageHandler = Substitute.For<IMessageHandler>();
    private readonly ILogger<StartCommand> _logger = Substitute.For<ILogger<StartCommand>>();
    private readonly Update _updateObject = Substitute.For<Update>();


    [Fact]
    public void InstantiateStartCommand_NextStepNothing_WhenNotCallExecuteAsync()
    {
        StartCommand startCommand = new(_logger, _messageHandler);
        startCommand.NextStep.Should().Be(Steps.Nothing);
    }

    [Fact]
    public async void StartCommand_NextStepNothing_WhenCallExecuteAsyncAndMessageIsInvalid()
    {
        StartCommand startCommand = new(_logger, _messageHandler);
        var result = await startCommand.ExecuteAsync(new object());
        startCommand.NextStep.Should().Be(Steps.Nothing);
        result.Should().BeFalse();
    }

    [Fact]
    public async void StartComand_NextStepTest_WhenCallExecuteAsyncAndMessageIsValid()
    {
        _messageHandler.SentMessage(_updateObject, Arg.Any<string>()).Returns(true);

        StartCommand startCommand = new(_logger, _messageHandler);
        var result = await startCommand.ExecuteAsync(_updateObject);
        startCommand.NextStep.Should().Be(Steps.Test);
        result.Should().BeTrue();
    }

    [Fact]
    public async void StartCommand_NextStepNothing_WhenCallExecuteAsyncAndMessageIsInValid()
    {
        _messageHandler.SentMessage(_updateObject, Arg.Any<string>()).Returns(false);

        StartCommand startCommand = new(_logger, _messageHandler);
        var result = await startCommand.ExecuteAsync(_updateObject);
        startCommand.NextStep.Should().Be(Steps.Nothing);
        result.Should().BeFalse();
    }
}
