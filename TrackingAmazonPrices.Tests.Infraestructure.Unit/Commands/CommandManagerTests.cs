using Xunit;
using NSubstitute;
using TrackingAmazonPrices.Infraestructure.Commands;
using FluentAssertions;
using System.Collections.Generic;
using TrackingAmazonPrices.Application.Command;
using Microsoft.Extensions.Logging;
using TrackingAmazonPrices.Application.Handlers;
using System.Linq;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Commands;

[Collection("pito")]
public class CommandManagerTests
{
    //bool IsCommand(string command);
    //ICommand GetCommand(string messageCommand);
    //ICommand GetNextCommand(Steps? step);
    //ICommand NullCommand();
    static IMessageHandler messageHandler = Substitute.For<IMessageHandler>();

    static IEnumerable<ICommand> commands = new List<ICommand>
        {
            new StartCommand(Substitute.For<ILogger<StartCommand>>(), messageHandler),
            new TestCommand(Substitute.For<ILogger<TestCommand>>(), messageHandler),
            new NullCommand(),
        };

    private readonly CommandManager _sut;

    public CommandManagerTests()
    {
        _sut = new CommandManager(commands);
    }

    [Theory]
    [InlineData("/start")]
    [InlineData("/test")]
    public void IsValidCommand_True_WhenCommandIsValid(string commandMessage)
    {
        var result = _sut.IsCommand(commandMessage);
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNotValidCommand_ReturnFalse_WhenCommandIsNotValid()
    {
        string invalidCommand = "/wiii";
        var result = _sut.IsCommand(invalidCommand);
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("/start")]
    [InlineData("/test")]
    public void GetCommand_ReturnCommand_WhenCommandIsValid(string commandMessage)
    {
        var result = _sut.GetCommand(commandMessage);
        result.Should().BeAssignableTo<ICommand>();
    }

    [Fact]
    public void GetCommand_ReturnNullCommandObject_WhenCommandIsNotValid()
    {
        string invalidCommand = "/wiii";

        var result = _sut.GetCommand(invalidCommand);

        result.Should().BeAssignableTo<ICommand>();
        result.Should().BeOfType<NullCommand>();
    }
    
    [Fact]
    public void GetNullCommand_ReturnNullCommandObject_WhenRequestForNullCommand()
    {

        var result = _sut.NullCommand();

        result.Should().BeAssignableTo<ICommand>();
        result.Should().BeOfType<NullCommand>();
    }
        
    [Fact]
    public void GetNextCommand_ReturnAnotherCommand_WhenCallWithCommandWithNextStep()
    {
        var startCommand = commands.First();
        var result = _sut.GetNextCommand(startCommand.NextStep);

        result.Should().BeAssignableTo<ICommand>();
    }
            
    [Fact]
    public void GetNextCommand_ReturnNullCommandObject_WhenCallWithCommandWithoutNextStep()
    {
        var startCommand = commands.First();
        var result = _sut.GetNextCommand(startCommand.NextStep);

        result.Should().BeAssignableTo<ICommand>();
        result.Should().BeOfType<NullCommand>();
    }

}