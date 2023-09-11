using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NSubstitute;
using NSubstitute.Core.Arguments;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.ConsoleApp;
using TrackingAmazonPrices.Domain.Enums;
using TrackingAmazonPrices.Infraestructure.Commands;
using Xunit;

namespace TrackingAmazonPrices.Tests.ConsoleApp.Unit;

public class ControllerMessagesTests
{
    private readonly ILogger<ControllerMessages> _logger = Substitute.For<ILogger<ControllerMessages>>();
    private readonly ILogger<StartCommand> _loggerStart = Substitute.For<ILogger<StartCommand>>();
    private readonly ICommandManager _commandManager = Substitute.For<ICommandManager>();
    private readonly IMessageHandler _messageHandler = Substitute.For<IMessageHandler>();
    private readonly IPoolingCommands _poolingCommands = Substitute.For<IPoolingCommands>();

    [Fact]
    public void HandlerMessageImp_CheckCalls_WhenReceiveUpdateMessage_WithValidCommand()
    {

        ControllerMessages sut = Substitute.ForPartsOf<ControllerMessages>
            (_logger, _commandManager, _messageHandler, _poolingCommands);


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

        ICommand startCommand = Substitute.ForPartsOf<StartCommand>(_loggerStart, _messageHandler);

        _messageHandler.GetTypeMessage(update).Returns(MessageTypes.Command);
        _messageHandler.SentInlineKeyboardMessage(Arg.Any<Update>(), Arg.Any<string>(), Arg.Any<object>()).Returns(true);
        _messageHandler.IsValidMessage(update).Returns(true);
        _messageHandler.GetMessage(update).Returns(update.Message.Text);
        _messageHandler.GetChatId(update).Returns(update.Message.Chat.Id);
        _commandManager.IsCommand(update.Message.Text).Returns(true);
        _commandManager.GetCommand(update.Message.Text).Returns(startCommand);
        startCommand.ExecuteAsync(update).Returns(Task.FromResult(true));

        sut.HandlerMessageImp(update);

        _messageHandler.Received().IsValidMessage(update);
        _messageHandler.Received().GetMessage(update);
        _messageHandler.Received().GetChatId(update);
        _poolingCommands.Received().TryAddCommand(Arg.Any<long>(), Arg.Any<ICommand>());
    }

    [Fact]
    public void HandlerMessageImp_CheckCalls_WhenReceiveCallBakc_WithCommand()
    {
        ControllerMessages sut = Substitute.ForPartsOf<ControllerMessages>
            (_logger, _commandManager, _messageHandler, _poolingCommands);

        CallbackQuery callBack = new();

        _messageHandler.GetTypeMessage(Arg.Any<object>()).Returns(MessageTypes.CallbackQuery);
        _messageHandler.IsCallBackQuery(callBack).Returns(true);

        sut.HandlerMessageImp(callBack);

        _messageHandler.Received().IsCallBackQuery(callBack);
    }
}
