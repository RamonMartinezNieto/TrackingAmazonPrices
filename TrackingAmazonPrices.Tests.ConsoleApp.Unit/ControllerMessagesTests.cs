using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Core.Arguments;
using System;
using Telegram.Bot.Types;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.ConsoleApp;
using TrackingAmazonPrices.Infraestructure.Commands;
using Xunit;

namespace TrackingAmazonPrices.Tests.ConsoleApp.Unit;

public class ControllerMessagesTests
{
    private readonly ILogger<ControllerMessages> _logger = Substitute.For<ILogger<ControllerMessages>>();
    private readonly ILogger<TestCommand> _loggerTestCommand = Substitute.For<ILogger<TestCommand>>();
    private readonly ICommandManager _commandMananger = Substitute.For<ICommandManager>();
    private readonly IMessageHandler _messageHandler = Substitute.For<IMessageHandler>();
    private readonly IPoolingCommands _poolingCommands = Substitute.For<IPoolingCommands>();

    [Fact]
    public void HandlerMessageImp_CheckCalls_WhenReceiveUpdateMessage_WithCommand()
    {
        TestCommand testCommand = new TestCommand(_loggerTestCommand, _messageHandler);

        ControllerMessages sut = Substitute.ForPartsOf<ControllerMessages>
            (_logger, _commandMananger, _messageHandler, _poolingCommands);


        Update update = new()
        {
            Message = new()
            {
                Text = "/start",
                Chat = new()
                {
                    Id = 123L
                }
            },
        };

        _messageHandler.IsValidMessage(update).Returns(true);

        sut.HandlerMessageImp(update);

        _messageHandler.Received().IsCallBackQuery(update);
        _messageHandler.Received().IsValidMessage(update);
        _messageHandler.Received().GetMessage(update);
        _messageHandler.Received().GetChatId(update);
        _poolingCommands.Received().TryAddCommand(Arg.Any<long>(), Arg.Any<ICommand>());
    }

    [Fact]
    public void HandlerMessageImp_CheckCalls_WhenReceiveCallBakc_WithCommand()
    {
        TestCommand testCommand = new TestCommand(_loggerTestCommand, _messageHandler);

        ControllerMessages sut = Substitute.ForPartsOf<ControllerMessages>
            (_logger, _commandMananger, _messageHandler, _poolingCommands);

        CallbackQuery callBack = new();

        _messageHandler.IsCallBackQuery(callBack).Returns(true);

        sut.HandlerMessageImp(callBack);

        _messageHandler.Received().IsCallBackQuery(callBack);
    }
}
