using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Domain.Entities;
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
        await _handlerMessage.Received(1).GetUser(Arg.Any<object>());
        await _handlerMessage.Received(1).AnswerdCallback(user, "Que tengas un buen día!");
        await _literals.Received(1).GetAsync(LanguageType.Spanish, Literals.GoodDay);
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
        await _handlerMessage.Received(1).GetUser(Arg.Any<object>());
        await _handlerMessage.Received(1).AnswerdCallback(Arg.Any<object>(), "You don't have a registered user");
        await _literals.Received(1).GetAsync(Literals.NoUser);
        await _userService.Received(1).UserExists(user.UserId);
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
        await _handlerMessage.Received(1).GetUser(Arg.Any<object>());
        await _handlerMessage.Received(1).AnswerdCallback(Arg.Any<object>(), "Usuario eliminado Que tengas un buen día!");
        await _literals.Received(1).GetAsync(LanguageType.Spanish, Literals.UserDeleted);
        await _literals.Received(1).GetAsync(LanguageType.Spanish, Literals.GoodDay);
        await _userService.Received(1).UserExists(user.UserId);
        await _userService.Received(1).DeleteUser(user.UserId);
    }

    private static Domain.Entities.User Getuser()
        => new(
            "Ramon",
            123L,
            PlatformType.Telegram,
            LanguageType.Spanish);
}
