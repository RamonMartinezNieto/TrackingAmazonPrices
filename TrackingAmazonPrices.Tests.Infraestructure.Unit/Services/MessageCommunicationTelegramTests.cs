using Microsoft.Extensions.Options;
using NSubstitute.Core.Arguments;
using System.Threading;
using Telegram.Bot.Polling;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Services;

public class MessageCommunicationTelegramTests
{
    private readonly ILogger<MessageCommunicationTelegram> _logger = Substitute.For<ILogger<MessageCommunicationTelegram>>();
    private readonly ILogger<HandlerMessageTelegram> _loggerHandler = Substitute.For<ILogger<HandlerMessageTelegram>>();
    private readonly IBotClient<ITelegramBotClient> _botClient = Substitute.For<IBotClient<TelegramBotClient>>();
    private readonly IControllerMessage _controllerMessage = Substitute.For<IControllerMessage>();


    [Fact]
    public void StartComunication_ThrowArgumentException_WhenHandlerIsNotHandlerMessageTelegram()
    {
        MessageCommunicationTelegram _sut = new(_logger, _botClient, null, _controllerMessage);

        Action act = () => _sut.StartComunication();

        act.Should().Throw<ArgumentException>()
            .WithMessage("The handler is not valid for bot telegram");
    }

    [Fact]
    public void StartComunication_ReturnIHandlerMessage_WhenStartCommunication()
    {
        TelegramBotClient telegramBotClient = new("some_token");
        IMessageHandler handler = new HandlerMessageTelegram(_loggerHandler, _botClient);

        _botClient.BotClient.Returns(telegramBotClient);

        MessageCommunicationTelegram _sut = new(_logger, _botClient, handler, _controllerMessage);
        var result = _sut.StartComunication();

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IMessageHandler>();
        result.Should().BeEquivalentTo(handler);
    }
}