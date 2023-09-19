namespace TrackingAmazonPrices.Domain.Definitions;

public static class CallbackMessages
{
    private const char SPLITTER = '_';

    public const string LANGUAGE_CALLBACK = "/callbackLanguage";
    public const string DELETE_USER_CALLBACK = "/callbackDeleteUser";

    public static string GetCallback(string callback, string data) 
        => callback + SPLITTER + data;
}
