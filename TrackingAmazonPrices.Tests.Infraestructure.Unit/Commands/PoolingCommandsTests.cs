using TrackingAmazonPrices.Application;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Commands;

public class PoolingCommandsTests
{
    private static readonly IMessageHandler _messageHandler = Substitute.For<IMessageHandler>();
    private static readonly IDatabaseUserService _databaseUserHandler = Substitute.For<IDatabaseUserService>();
    private static readonly ILiteralsService _literalsService = Substitute.For<ILiteralsService>();

    private readonly StartCommand _startCommand = new(Substitute.For<ILogger<StartCommand>>(), _messageHandler,_literalsService);
    private readonly TestCommand _testCommand = new(Substitute.For<ILogger<TestCommand>>(), _messageHandler, _databaseUserHandler, _literalsService);
    private readonly NullCommand _nullCommand = new();
    private readonly PoolingCommands _sut;
    private static readonly long _chatId = 123456L;

    public PoolingCommandsTests()
    {
        _sut = new PoolingCommands();
    }

    [Fact]
    public void TryAddCommand_ReturnTrue_WhenCommandIsNotNullCommandObject()
    {
        var result = _sut.TryAddCommand(_chatId, _startCommand);
        result.Should().BeTrue();
    }

    [Fact]
    public void TryAddCommand_ReturnFalse_WhenCommandIsNullCommandObject()
    {
        var result = _sut.TryAddCommand(_chatId, _nullCommand);
        result.Should().BeFalse();
    }

    [Fact]
    public void TryAddCommand_ReturnFalse_WhenCommandIsNull()
    {
        var result = _sut.TryAddCommand(_chatId, null);
        result.Should().BeFalse();
    }

    [Fact]
    public void TryGetPendingCommand_ReturnCommand_WhenCommandExists()
    {
        var addCommand = _sut.TryAddCommand(_chatId, _testCommand);
        var result = _sut.TryGetPendingCommandResponse(_chatId, out ICommand commandResult);

        addCommand.Should().BeTrue();
        result.Should().BeTrue();

        commandResult.Should().BeAssignableTo<ICommand>();
        commandResult.Should().BeOfType<TestCommand>();
    }

    [Fact]
    public void TryGetPendingCommand_ReturnTrue_WhenCommandNotExists()
    {
        var result = _sut.TryGetPendingCommandResponse(_chatId, out ICommand commandResult);

        result.Should().BeFalse();
        commandResult.Should().BeNull();
    }
}