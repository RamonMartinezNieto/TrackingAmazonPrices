using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TrackingAmazonPrices.Domain.Exceptions;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Handlers;

public class HandlerMessageTelegramTest
{
    private readonly IBotClient<ITelegramBotClient> _botClient = Substitute.For<IBotClient<ITelegramBotClient>>();
    private readonly ILogger<HandlerMessageTelegram> _logger = Substitute.For<ILogger<HandlerMessageTelegram>>();
    private readonly IControllerMessage _controllerMessage = Substitute.For<IControllerMessage>();

    private readonly HandlerMessageTelegram _sut;

    public HandlerMessageTelegramTest()
    {
        _sut = new(_logger, _botClient);
    }

    [Fact]
    public void SetController_ThrowException_WhenIsNotInvalid()
    {
        Action act = () => _sut.SetControllerMessage(null);

        act.Should().Throw<InvalidControllerException>()
            .WithMessage("Controller Message is not defined, call method SetControllerMessage");
    }

    [Fact]
    public void SetController_ReturnController_WhenIsValid()
    {
        var result = _sut.SetControllerMessage(_controllerMessage);

        result.Should().BeAssignableTo<IControllerMessage>();
        result.Should().BeEquivalentTo(_controllerMessage);
    }

    [Fact]
    public void IsValidMessage_ReturnTrue_WhenIsUpdateWithTextMessage()
    {
        Update message = GetMockMessage();
        var result = _sut.IsValidMessage(message);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidMessage_ReturnFalse_UpdateIsCallback()
    {
        var callback = GetMockCallback();

        var result = _sut.IsValidMessage(callback);

        result.Should().BeFalse();
    }

    
    [Fact]
    public void IsCallback_ReturnTrue_WhenIsUpdateIsCallback()
    {
        var callback = GetMockCallback();

        var result = _sut.IsCallBackQuery(callback);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsCallback_ReturnFalse_WhenIsUpdateIsMessage()
    {
        Update message = GetMockMessage();

        var result = _sut.IsCallBackQuery(message);

        result.Should().BeFalse();
    }

    [Fact]
    public void GetMessage_ReturnText_WhenIsMessageWithSomeText() 
    {
        Update message = GetMockMessage();

        var result = _sut.GetMessage(message);

        result.Should().Be("Some text message");
    }
    
    [Fact]
    public void GetMessage_ReturnEmpty_WhenIsNotMessage() 
    {
        Update message = GetMockCallback();

        var result = _sut.GetMessage(message);

        result.Should().Be(string.Empty);
    }
    
    [Fact]
    public void GetChatId_ReturnLongId_WhenUpdateIsMessage() 
    {
        Update message = GetMockMessage();

        var result = _sut.GetChatId(message);

        result.Should().Be(123L);
    }
    
    [Fact]
    public void GetMessage_ReturnDefault_WhenIsNotMessage() 
    {
        Update message = GetMockCallback();

        var result = _sut.GetChatId(message);

        result.Should().Be(default);
    }    

    [Fact]
    public void SentMessage_ThrowException_InvalidObject() 
    {
        object someObject = new();

        Func<Task> act = async () => await _sut.SentMessage(someObject, Arg.Any<string>());

        act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("invalid objectMessage, this is not Update for telegram client");
    }
    
    [Fact]
    public void SentMessage_NotThrowException_ValidObject() 
    {
        Update message = GetMockCallback();

        Func<Task> act = async () => await _sut.SentMessage(message, Arg.Any<string>());

        act.Should().NotThrowAsync<ArgumentException>();
        _botClient.BotClient.SendTextMessageAsync(
            chatId: Arg.Any<long>(),
            text: Arg.Any<string>(),
            disableNotification: Arg.Any<bool>(),
            parseMode: Arg.Any<ParseMode>());
    }

    private static Update GetMockMessage()
    {
        Update message = new()
        {
            Message = new()
        };
        message.Message.Text = "Some text message";
        message.Message.Chat = new()
        {
            Id = 123L
        };

        return message;
    }

    private static Update GetMockCallback() => new()
    {
        CallbackQuery = new()
    };
}