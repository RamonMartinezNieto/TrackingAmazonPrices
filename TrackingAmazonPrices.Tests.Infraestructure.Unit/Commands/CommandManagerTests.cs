using TrackingAmazonPrices.Application;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Commands;

public class CommandManagerTests
{
    private static readonly IMessageHandler _messageHandler = Substitute.For<IMessageHandler>();
    private static readonly ILiteralsService _literalsService = Substitute.For<ILiteralsService>();
    private static readonly IDatabaseUserService _userService = Substitute.For<IDatabaseUserService>();

    private static readonly IEnumerable<ICommand> _commands = new List<ICommand>
        {
            new StartCommand(Substitute.For<ILogger<StartCommand>>(), _messageHandler, _literalsService, _userService),
            new LanguageCommand(Substitute.For<ILogger<LanguageCommand>>(), _messageHandler, _literalsService),
            new NullCommand(),
        };

    private readonly CommandManager _sut;

    public CommandManagerTests()
    {
        _sut = new CommandManager(_commands);
    }

    [Theory]
    [InlineData("/start")]
    [InlineData("/language")]
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
    [InlineData("/language")]
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
        var startCommand = _commands.First();
        var result = _sut.GetNextCommand(startCommand.NextStep);

        result.Should().BeAssignableTo<ICommand>();
    }

    [Fact]
    public void GetNextCommand_ReturnNullCommandObject_WhenCallWithCommandWithoutNextStep()
    {
        var startCommand = _commands.First();
        var result = _sut.GetNextCommand(startCommand.NextStep);

        result.Should().BeAssignableTo<ICommand>();
        result.Should().BeOfType<NullCommand>();
    }
}