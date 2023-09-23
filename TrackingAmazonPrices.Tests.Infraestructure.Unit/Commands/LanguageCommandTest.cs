using Telegram.Bot.Types;
using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Tests.Infraestructure.Unit.Mocks;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Commands;

public class LanguageCommandTest
{
    private readonly IMessageHandler _messageHandler = Substitute.For<IMessageHandler>();
    private readonly ILiteralsService _literalsService = Substitute.For<ILiteralsService>();
    private readonly ILogger<LanguageCommand> _logger = Substitute.For<ILogger<LanguageCommand>>();
    private readonly Update _updateObject = Substitute.For<Update>();

    private readonly LanguageCommand _sut;

    public LanguageCommandTest()
    {
        _sut = new(_logger, _messageHandler, _literalsService);
    }

    [Fact]
    public async void StartComand_NextStepTest_WhenCallExecuteAsyncAndMessageIsValid()
    {
        ICommand languageCommand = Substitute.For<ICommand>();

        _messageHandler.GetUser(Arg.Any<object>()).Returns(UserMocks.GetUserWithLanguage());
        _messageHandler.SentInlineKeyboardMessage(_updateObject, Arg.Any<string>(), Arg.Any<object>()).Returns(true);
        _messageHandler.SentMessageAsync(_updateObject, Arg.Any<string>()).Returns(true);
        languageCommand.ExecuteAsync(Arg.Any<object>()).Returns(true);

        var result = await _sut.ExecuteAsync(_updateObject);

        await _messageHandler.Received(1).SentInlineKeyboardMessage(_updateObject, Arg.Any<string>(), Arg.Any<object>());

        result.Should().BeTrue();
    }

    [Fact]
    public async void StartCommand_NextStepNothing_WhenCallExecuteAsyncAndMessageIsInValid()
    {
        _messageHandler.GetUser(Arg.Any<object>()).Returns(UserMocks.GetUserWithLanguage());
        _messageHandler.SentInlineKeyboardMessage(_updateObject, Arg.Any<string>(), Arg.Any<object>()).Returns(false);

        var result = await _sut.ExecuteAsync(_updateObject);

        result.Should().BeFalse();
    }
}