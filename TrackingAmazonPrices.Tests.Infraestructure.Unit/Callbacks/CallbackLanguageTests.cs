using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Infraestructure.Callbacks;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Callbacks;

public class CallbackLanguageTests
{
    private readonly ILogger<LanguageCallback> _logger = Substitute.For<ILogger<LanguageCallback>>();
    private readonly IMessageHandler _messageHandler   = Substitute.For<IMessageHandler>();
    private readonly ILiteralsService _literals        = Substitute.For<ILiteralsService>();
    private readonly IDatabaseUserService _userService = Substitute.For<IDatabaseUserService>();

    private readonly LanguageCallback _sut; 

    public CallbackLanguageTests()
    {
        _sut = new LanguageCallback(_logger, _messageHandler, _literals, _userService);
    }

    [Fact]
    public async Task ExecuteAsync_UpdatesLanguageAndUser_Success()
    {

        var objectMessage = new object();
        var dataCallback = "en";

        var user = new Domain.Entities.User();
        _messageHandler.GetUser(objectMessage).Returns(Task.FromResult(user));
        _userService.SaveUserAsync(user).Returns(true);
        _literals.GetAsync(Arg.Any<LanguageType>(), Arg.Any<Literals>()).Returns(Task.FromResult("Update successful"));

        var result = await _sut.ExecuteAsync(objectMessage, dataCallback);

        result.Should().BeTrue();
        user.Language.LanguageCode.Should().Be(LanguageType.English);
        await _messageHandler.Received().AnswerdCallback(objectMessage, "Update successful");
        await _messageHandler.Received().SentMessageAsync(objectMessage, "Update successful");
    }

    [Fact]
    public async Task ExecuteAsync_FailsToUpdateUser_LogsWarning()
    {
        var objectMessage = new object();
        var dataCallback = "es";

        var user = new Domain.Entities.User();
        _messageHandler.GetUser(objectMessage).Returns(Task.FromResult(user));
        _userService.SaveUserAsync(user).Returns(false);

        var result = await _sut.ExecuteAsync(objectMessage, dataCallback);

        result.Should().BeFalse();
        await _messageHandler.DidNotReceive().AnswerdCallback(Arg.Any<object>(), Arg.Any<string>());
        await _messageHandler.DidNotReceive().SentMessageAsync(Arg.Any<object>(), Arg.Any<string>());
    }
}
