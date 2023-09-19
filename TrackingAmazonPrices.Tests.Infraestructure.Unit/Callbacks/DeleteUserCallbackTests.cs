using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Domain;
using TrackingAmazonPrices.Domain.Enums;
using TrackingAmazonPrices.Infraestructure.Callbacks;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Callbacks;

public class DeleteUserCallbackTests
{
    private readonly ILogger<DeleteUserCallback> _logger = Substitute.For<ILogger<DeleteUserCallback>>();
    private readonly IMessageHandler _handlerMessage = Substitute.For<IMessageHandler>();
    private readonly ILiteralsService _literals = Substitute.For<ILiteralsService>();
    private readonly IDatabaseUserService _userService = Substitute.For<IDatabaseUserService>();

    private readonly DeleteUserCallback _sut;

    public DeleteUserCallbackTests()
    {
        _sut = new DeleteUserCallback(_logger, _handlerMessage, _userService, _literals);
    }

    [Fact]
    public async Task ExecuteAsync_AnswerdGoodDayReturnTrue_WhenDataIsNo()
    {
        var user = Getuser();
        var dataCallback = "no";

        _handlerMessage
            .GetUser(Arg.Any<object>())
            .Returns(user);

        _literals
            .GetAsync(LanguageType.Spanish, Literals.GoodDay)
            .Returns("Que tengas un buen día!");

        _handlerMessage
            .AnswerdCallback(user, "Que tengas un buen día!")
            .Returns(true);

        var result = await _sut.ExecuteAsync(user, dataCallback);

        result.Should().BeTrue();
        _handlerMessage.Received(2);
        _literals.Received(1);

        _userService.DidNotReceive();
        _logger.DidNotReceive();
    }
    
    [Fact]
    public async Task ExecuteAsync_AnswerdNoUser_True_WhenUserNotExists()
    {
        var user = Getuser();
        var dataCallback = "yes";

        _handlerMessage
            .GetUser(Arg.Any<object>())
            .Returns(user);

        _userService
            .UserExists(user.UserId)
            .Returns(false);

        _literals
            .GetAsync(Literals.NoUser)
            .Returns("You don't have a registered user");

        _handlerMessage
            .AnswerdCallback(Arg.Any<object>(), "You don't have a registered user")
            .Returns(true);

        var result = await _sut.ExecuteAsync(user, dataCallback);

        result.Should().BeTrue();
        _handlerMessage.Received(2);
        _literals.Received(1);
        _userService.Received(1);
        _logger.DidNotReceive();
    }    

    [Fact]
    public async Task ExecuteAsync_AnswerdUserDeleted_True_WhenUserExistsAndDataIsYes()
    {
        var user = Getuser();
        var dataCallback = "yes";

        _handlerMessage
            .GetUser(Arg.Any<object>())
            .Returns(user);

        _userService
            .UserExists(user.UserId)
            .Returns(true);

        _userService
            .DeleteUser(user.UserId)
            .Returns(true);

        _literals
            .GetAsync(LanguageType.Spanish, Literals.UserDeleted)
            .Returns("Usuario eliminado");

        _literals
            .GetAsync(LanguageType.Spanish, Literals.GoodDay)
            .Returns("Que tengas un buen día!");

        _handlerMessage
             .AnswerdCallback(Arg.Any<object>(), "Usuario eliminado Que tengas un buen día!")
             .Returns(true);

        var result = await _sut.ExecuteAsync(user, dataCallback);

        result.Should().BeTrue();
        _handlerMessage.Received(2);
        _literals.Received(2);
        _userService.Received(1);
        _logger.Received(2);
    }

    private static Domain.Entities.User Getuser()
        => new(
            "Ramon",
            123L,
            PlatformType.Telegram,
            new Language(LanguageType.Spanish));
}
