using TrackingAmazonPrices.Application;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Commands;

public class StartCommandTests
{
    private readonly IMessageHandler _messageHandler = Substitute.For<IMessageHandler>();
    private readonly ILiteralsService _literalsService = Substitute.For<ILiteralsService>();
    private readonly ILogger<StartCommand> _logger = Substitute.For<ILogger<StartCommand>>();
    private readonly Update _updateObject = Substitute.For<Update>();

    private readonly StartCommand _sut;

    public StartCommandTests()
    {
        _sut = new(_logger, _messageHandler, _literalsService);
    }

    [Fact]
    public async void StartCommand_NextStepNothing_WhenCallExecuteAsyncAndMessageIsInvalid()
    {
        _messageHandler.GetUser(Arg.Any<object>()).Returns(GetUserWithLanguage());
        var result = await _sut.ExecuteAsync(new object());
        _sut.NextStep.Should().Be(Steps.Nothing);
        result.Should().BeFalse();
    }

    [Fact]
    public async void StartComand_NextStepTest_WhenCallExecuteAsyncAndMessageIsValid()
    {
        _messageHandler.GetUser(Arg.Any<object>()).Returns(GetUserWithLanguage());
        _messageHandler.SentInlineKeyboardMessage(_updateObject, Arg.Any<string>(), Arg.Any<object>()).Returns(true);
        _messageHandler.SentMessage(_updateObject, Arg.Any<string>()).Returns(true);

        var result = await _sut.ExecuteAsync(_updateObject);

        await _messageHandler.Received(1).SentMessage(_updateObject, Arg.Any<string>());
        await _messageHandler.Received(1).SentInlineKeyboardMessage(_updateObject, Arg.Any<string>(), Arg.Any<object>());
        _sut.NextStep.Should().Be(Steps.Test);
        result.Should().BeTrue();
    }


    [Fact]
    public async void StartCommand_NextStepNothing_WhenCallExecuteAsyncAndMessageIsInValid()
    {
        _messageHandler.GetUser(Arg.Any<object>()).Returns(GetUserWithLanguage());
        _messageHandler.SentMessage(_updateObject, Arg.Any<string>()).Returns(false);

        var result = await _sut.ExecuteAsync(_updateObject);

        await _messageHandler.DidNotReceive().SentInlineKeyboardMessage(_updateObject, Arg.Any<string>(), Arg.Any<object>());

        _sut.NextStep.Should().Be(Steps.Nothing);
        result.Should().BeFalse();
    }


    private static Domain.Entities.User GetUserWithLanguage()
    {
        return new()
        {
            Language = new Domain.Entities.Language()
            {
                LanguageCode = LanguageType.English
            },
        };
    }
}