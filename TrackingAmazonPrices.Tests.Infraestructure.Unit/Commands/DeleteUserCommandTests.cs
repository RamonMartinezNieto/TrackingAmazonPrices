﻿using Telegram.Bot.Types.ReplyMarkups;
using TrackingAmazonPrices.Application;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Commands;

public class DeleteUserCommandTests
{
    private readonly ILogger<DeleteUserCommand> _logger = Substitute.For<ILogger<DeleteUserCommand>>();
    private readonly IDatabaseUserService _databaseUserService = Substitute.For<IDatabaseUserService>();
    private readonly IMessageHandler _messageHandler = Substitute.For<IMessageHandler>();
    private readonly ILiteralsService _literalsService = Substitute.For<ILiteralsService>();

    private readonly DeleteUserCommand _sut;

    public DeleteUserCommandTests()
    {
        _sut = new(_logger, _databaseUserService, _messageHandler, _literalsService);
    }

    [Fact]
    public async Task Execute_SentMessageNoUser_WhenThereIsNotUserInDatabase()
    {
        var user = Getuser();

        _messageHandler
            .GetUser(user)
            .Returns(user);
        _literalsService
            .GetAsync(Literals.NoUser)
            .Returns("You don't have a registered user");
        _databaseUserService
            .UserExists(user.UserId)
            .Returns(false);
        _messageHandler
            .SentMessageAsync(Arg.Any<object>(), "You don't have a registered user")
            .Returns(true);

        var result = await _sut.ExecuteAsync(user);

        result.Should().BeTrue();
        _messageHandler.Received(2);
        _databaseUserService.Received(1);
        _literalsService.Received(1);
    }


    [Fact]
    public async Task Execute_SentMenu_WhenExistsUser()
    {
        var user = Getuser();

        _messageHandler
            .GetUser(user)
            .Returns(user);
        _databaseUserService
            .UserExists(user.UserId)
            .Returns(true);
        _messageHandler
            .SentInlineKeyboardMessage(user, Arg.Any<string>(), Arg.Any<InlineKeyboardMarkup>())
            .Returns(true);

        var result = await _sut.ExecuteAsync(user);

        result.Should().BeTrue();
        _messageHandler.Received(2);
        _databaseUserService.Received(1);
        _literalsService.Received(1);

        _logger.Received(1);
    }

    private static Domain.Entities.User Getuser()
    {
        return new(
            "Ramon",
            123L,
            PlatformType.Telegram);
    }
}
