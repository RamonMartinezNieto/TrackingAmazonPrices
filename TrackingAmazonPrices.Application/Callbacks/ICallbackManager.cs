namespace TrackingAmazonPrices.Application.Callbacks;

public interface ICallbackManager
{
    bool IsCallback(string command);

    ICallback GetCallback(string message);

    ICallback NullCallback();

    string GetData(string message);
}