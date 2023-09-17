using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Application.Command;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.ConsoleApp;
using TrackingAmazonPrices.Domain.Enums;
using TrackingAmazonPrices.Domain;
using TrackingAmazonPrices.Infraestructure.Commands;
using Xunit;
using TrackingAmazonPrices.Application.Callbacks;
using TrackingAmazonPrices.Application.Services;

namespace TrackingAmazonPrices.Tests.ConsoleApp.Unit;

public class ControllerMessagesTests
{
    private readonly ILogger<ControllerMessages> _logger = Substitute.For<ILogger<ControllerMessages>>();
    private readonly ILogger<StartCommand> _loggerStart = Substitute.For<ILogger<StartCommand>>();
    private readonly ICommandManager _commandManager = Substitute.For<ICommandManager>();
    private readonly IMessageHandler _messageHandler = Substitute.For<IMessageHandler>();
    private readonly IPoolingCommands _poolingCommands = Substitute.For<IPoolingCommands>();
    private readonly ILiteralsService _literals = Substitute.For<ILiteralsService>();
    private readonly ICallbackManager _callbackManager = Substitute.For<ICallbackManager>();
    private readonly IDatabaseUserService _userService = Substitute.For<IDatabaseUserService>();

    [Fact]
    public void HandlerMessageImp_CheckCalls_WhenReceiveUpdateMessage_WithValidCommand()
    {
        ControllerMessages sut = Substitute.ForPartsOf<ControllerMessages>
            (_logger, _commandManager, _callbackManager, _messageHandler, _poolingCommands);

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

        Domain.Entities.User user = new()
        {
            Language = new Language() 
            {
                LanguageCode = LanguageType.English
            },
        };

        ICommand startCommand = Substitute.ForPartsOf<StartCommand>(_loggerStart, _messageHandler, _literals, _userService);

        _messageHandler.GetTypeMessage(update).Returns(MessageTypes.Command);
        _messageHandler.SentInlineKeyboardMessage(Arg.Any<Update>(), Arg.Any<string>(), Arg.Any<object>()).Returns(true);
        _messageHandler.IsValidMessage(update).Returns(true);
        _messageHandler.GetMessage(update).Returns(update.Message.Text);
        _messageHandler.GetChatId(update).Returns(update.Message.Chat.Id);
        _commandManager.IsCommand(update.Message.Text).Returns(true);
        _messageHandler.GetUser(Arg.Any<object>()).Returns(user);
        _commandManager.GetCommand(update.Message.Text).Returns(startCommand);
        startCommand.ExecuteAsync(update).Returns(Task.FromResult(true));

        sut.HandlerMessageImp(update);

        _messageHandler.Received().IsValidMessage(update);
        _messageHandler.Received().GetMessage(update);
        _messageHandler.Received().GetChatId(update);
    }

    [Fact]
    public void HandlerMessageImp_CheckCalls_WhenReceiveCallBakc_WithCommand()
    {
        ControllerMessages sut = Substitute.ForPartsOf<ControllerMessages>
            (_logger, _commandManager, _callbackManager,  _messageHandler, _poolingCommands);

        CallbackQuery callBack = new();

        _messageHandler.GetTypeMessage(Arg.Any<object>()).Returns(MessageTypes.CallbackQuery);
        _messageHandler.IsCallBackQuery(callBack).Returns(true);

        sut.HandlerMessageImp(callBack);

        _messageHandler.Received().IsCallBackQuery(callBack);
    }
}