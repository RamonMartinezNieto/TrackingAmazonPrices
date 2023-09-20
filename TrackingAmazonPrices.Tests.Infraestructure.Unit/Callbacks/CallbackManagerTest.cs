using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Application.Callbacks;
using TrackingAmazonPrices.Infraestructure.Callbacks;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Callbacks;

public class CallbackManagerTest
{
    private readonly ILogger<LanguageCallback> _logger = Substitute.For<ILogger<LanguageCallback>>();
    private readonly IMessageHandler _messageHandler = Substitute.For<IMessageHandler>();
    private readonly ILiteralsService _literals = Substitute.For<ILiteralsService>();
    private readonly IDatabaseUserService _userService = Substitute.For<IDatabaseUserService>();

    private readonly IEnumerable<ICallback> _callbackProvider;
    private readonly CallbackManager _sut;

    public CallbackManagerTest()
    {
        _callbackProvider = new List<ICallback>
        {
            new LanguageCallback(_logger, _messageHandler, _literals, _userService),
            new NullCallback()
        };

        _sut = new CallbackManager(_callbackProvider);
    }

    [Fact]
    public void GetCallback_ValidMessage_ReturnsCallback()
    {
        var validMessage = "/callbackLanguage_data";

        var callback = _sut.GetCallback(validMessage);

        callback.Should().BeOfType<LanguageCallback>();
    }

    [Fact]
    public void GetCallback_InvalidMessage_ReturnsNullCallback()
    {
        var invalidMessage = "/invalidCallback_data";

        var callback = _sut.GetCallback(invalidMessage);

        callback.Should().BeOfType<NullCallback>();
    }

    [Fact]
    public void GetData_ExtractsDataFromMessage_ReturnsData()
    {
        var message = "command_data";

        var data = _sut.GetData(message);

        data.Should().Be("data");
    }

    [Fact]
    public void IsCallback_ValidCommand_ReturnsTrue()
    {
        var validCommand = "/callbackLanguage_data";

        var isCallback = _sut.IsCallback(validCommand);

        isCallback.Should().BeTrue();
    }

    [Fact]
    public void IsCallback_InvalidCommand_ReturnsFalse()
    {
        var invalidCommand = "/invalidCallback_data";

        var isCallback = _sut.IsCallback(invalidCommand);

        isCallback.Should().BeFalse();
    }
}