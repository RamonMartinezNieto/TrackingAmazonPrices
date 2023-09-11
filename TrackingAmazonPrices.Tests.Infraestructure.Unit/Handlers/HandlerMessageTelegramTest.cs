﻿using NSubstitute.Core.Arguments;
using System.Threading;

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

    [Fact]
    public void SentMessageInline_ThrowException_InvalidObject() 
    {
        object someObject = new();

        Func<Task> act = async () => await _sut.SentInlineKeyboardMessage(someObject, Arg.Any<string>(), Arg.Any<object>());

        act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("invalid objectMessage, this is not Update for telegram client");
    }
    
    [Fact]
    public void SentMessageInline_NotThrowException_ValidObject() 
    {
        List<string[,]> menuRows = new()
        {
            new string[2, 2] {
                { "ES " + TelegramEmojis.SPAINT_FLAG, "ESP" },
                { "EN " + TelegramEmojis.GB_FLAG, "EN" } },
        };
        var menu = UtilsTelegramMessage.CreateMenu(menuRows);

        Update message = GetMockCallback();

        Func<Task> act = async () => await _sut.SentInlineKeyboardMessage(message, Arg.Any<string>(), menu);

        act.Should().NotThrowAsync<ArgumentException>();
        _botClient.BotClient.SendTextMessageAsync(
            chatId: Arg.Any<long>(),
            text: Arg.Any<string>(),
            disableNotification: Arg.Any<bool>(),
            parseMode: Arg.Any<ParseMode>());
    }

    [Fact]
    public void HandleUpdateAsync_ThrowInvalidController_WhenControllerNotValid() 
    {
        Update message = GetMockMessage();
        using CancellationTokenSource cts = new ();
        
        Func<Task> act = async () => await _sut.HandleUpdateAsync(_botClient.BotClient, message, cts.Token);

        act.Should()
            .ThrowAsync<InvalidControllerException>()
            .WithMessage("Controller Message is not defined, call method SetControllerMessage");
    }
        
    [Fact]
    public async Task HandleUpdateAsync_Execute_WhenControllerValid() 
    {
        Update message = GetMockMessage();
        using CancellationTokenSource cts = new ();

        _sut.SetControllerMessage(_controllerMessage);

        await _sut.HandleUpdateAsync(_botClient.BotClient, message, cts.Token);
        
        _controllerMessage.Received().HandlerMessage(Arg.Any<Update>());
    }

    [Fact]
    public async Task HandlePollingErrorAsync_ThrowException_WhenControllerNotValid()
    {
        using CancellationTokenSource cts = new();
        Exception someException = new("ups");

        _sut.SetControllerMessage(_controllerMessage);

        Func<Task> act = async () => await _sut.HandlePollingErrorAsync(_botClient.BotClient, someException, cts.Token);

        await act
            .Should()
            .ThrowAsync<Exception>();

        _controllerMessage.Received().HandlerError(someException);
    }


    [Fact]
    public void HandlePollingErrorAsync_Execute_WhenControllerIsValid()
    {
        using CancellationTokenSource cts = new();
        Exception someException = new("ups");

        _sut.SetControllerMessage(_controllerMessage);

        Func<Task> act = async () => await _sut.HandlePollingErrorAsync(_botClient.BotClient, someException, cts.Token);

        act.Should()
            .ThrowAsync<InvalidControllerException>()
            .WithMessage("Controller Message is not defined, call method SetControllerMessage");
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