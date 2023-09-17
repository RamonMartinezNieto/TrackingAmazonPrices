using TrackingAmazonPrices.Application.Callbacks;
using TrackingAmazonPrices.Infraestructure.Commands;

namespace TrackingAmazonPrices.Infraestructure.Callbacks;

internal class CallbackManager : ICallbackManager
{
    private readonly Dictionary<string, Type> _callbacks = new()
    {
        { "/callbackLanguage", typeof(CallbackLanguage) },
    };

    private readonly IEnumerable<ICallback> _callbackProvider;

    public CallbackManager(
        IEnumerable<ICallback> callbackProvider)
    {
        _callbackProvider = callbackProvider;
    }

    public ICallback GetCallback(string message)
    {
        string command = message.Split("_").First();

        if (_callbacks.TryGetValue(command, out var commandType))
        {
            return _callbackProvider.FirstOrDefault(x => x.GetType() == commandType);
        }

        return NullCallback();
    }

    public string GetData(string message)
        => message.Split("_").Last();

    public bool IsCallback(string command)
    {
        string commandKey = command.Split("_").First();
        return _callbacks.ContainsKey(commandKey);
    }

    public ICallback NullCallback()
        => _callbackProvider.FirstOrDefault(x => x.GetType() == typeof(NullCommand));
}