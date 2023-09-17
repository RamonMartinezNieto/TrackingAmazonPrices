using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Domain;
using TrackingAmazonPrices.Infraestructure.Callbacks;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Callbacks;

public class CallbackLanguageTests
{
    private readonly ILogger<CallbackLanguage> _logger = Substitute.For<ILogger<CallbackLanguage>>();
    private readonly IMessageHandler _messageHandler   = Substitute.For<IMessageHandler>();
    private readonly ILiteralsService _literals        = Substitute.For<ILiteralsService>();
    private readonly IDatabaseUserService _userService = Substitute.For<IDatabaseUserService>();

    private readonly CallbackLanguage _sut; 

    public CallbackLanguageTests()
    {
        _sut = new CallbackLanguage(_logger, _messageHandler, _literals, _userService);
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
