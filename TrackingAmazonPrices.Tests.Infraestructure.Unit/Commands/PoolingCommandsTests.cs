namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Commands;

//    static ConcurrentDictionary<long, ICommand> PoolingTestCommands { get; }
//    bool TryGetPendingCommandResponse(long chatId, out ICommand command);
//    bool TryAddCommand(long chatId, ICommand command);

public class PoolingCommandsTests
{
    private readonly static IMessageHandler _messageHandler = Substitute.For<IMessageHandler>();
    private readonly StartCommand _startCommand = new(Substitute.For<ILogger<StartCommand>>(), _messageHandler);
    private readonly TestCommand _testCommand = new(Substitute.For<ILogger<TestCommand>>(), _messageHandler);
    private readonly NullCommand _nullCommand = new();
    private readonly PoolingCommands _sut;

    public PoolingCommandsTests()
    {
        _sut = new PoolingCommands();
    }

    [Fact]
    public void TryAddCommand_ReturnTrue_WhenCommandIsNotNullCommandObject()
    {
        long chatId = 123456L;
        var result = _sut.TryAddCommand(chatId, _startCommand);
        result.Should().BeTrue();
    }
        
    [Fact]
    public void TryAddCommand_ReturnFalse_WhenCommandIsNullCommandObject()
    {
        long chatId = 123456L;
        var result = _sut.TryAddCommand(chatId, _nullCommand);
        result.Should().BeFalse();
    }
            
    [Fact]
    public void TryAddCommand_ReturnFalse_WhenCommandIsNull()
    {
        long chatId = 123456L;
        var result = _sut.TryAddCommand(chatId, null);
        result.Should().BeFalse();
    }

    [Fact]
    public void TryGetPendingCommand_ReturnCommand_WhenCommandExists()
    {
        long chatId = 123456L;
        var addCommand = _sut.TryAddCommand(chatId, _testCommand);
        var result = _sut.TryGetPendingCommandResponse(chatId, out ICommand commandResult);

        addCommand.Should().BeTrue();
        result.Should().BeTrue();

        commandResult.Should().BeAssignableTo<ICommand>();
        commandResult.Should().BeOfType<TestCommand>();
    }

    [Fact]
    public void TryGetPendingCommand_ReturnTrue_WhenCommandNotExists()
    {
        long chatId = 123456L;
        var result = _sut.TryGetPendingCommandResponse(chatId, out ICommand commandResult);

        result.Should().BeFalse();
        commandResult.Should().BeNull();
    }
}