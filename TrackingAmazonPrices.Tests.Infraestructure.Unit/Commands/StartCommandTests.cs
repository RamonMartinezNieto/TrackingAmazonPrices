using Telegram.Bot.Types;
using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Tests.Infraestructure.Unit.Mocks;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Commands;

public class StartCommandTests
{
    private readonly IMessageHandler _messageHandler = Substitute.For<IMessageHandler>();
    private readonly ILiteralsService _literalsService = Substitute.For<ILiteralsService>();
    private readonly ILogger<StartCommand> _logger = Substitute.For<ILogger<StartCommand>>();
    private readonly Update _updateObject = Substitute.For<Update>();
    private static readonly ICommandManager _commandManager = Substitute.For<ICommandManager>();
    private readonly IDatabaseUserService _userDatabase = Substitute.For<IDatabaseUserService>();

    private readonly StartCommand _sut;

    public StartCommandTests()
    {
        _sut = new(_logger, _messageHandler, _literalsService, _userDatabase);
    }

    [Fact]
    public async void StartCommand_NextStepNothing_WhenCallExecuteAsyncAndMessageIsInvalid()
    {
        _messageHandler.GetUser(Arg.Any<object>()).Returns(UserMocks.GetUserWithLanguage());
        var result = await _sut.ExecuteAsync(new object());

        result.Should().BeFalse();
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

        await _messageHandler.Received(1).SentMessageAsync(_updateObject, Arg.Any<string>());
        result.Should().BeTrue();
    }

    [Fact]
    public async void StartCommand_NextStepNothing_WhenCallExecuteAsyncAndMessageIsInValid()
    {
        _messageHandler.GetUser(Arg.Any<object>()).Returns(UserMocks.GetUserWithLanguage());
        _messageHandler.SentMessageAsync(_updateObject, Arg.Any<string>()).Returns(false);

        var result = await _sut.ExecuteAsync(_updateObject);

        await _messageHandler.DidNotReceive().SentInlineKeyboardMessage(_updateObject, Arg.Any<string>(), Arg.Any<object>());

        result.Should().BeFalse();
    }
}