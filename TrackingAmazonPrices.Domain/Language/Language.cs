namespace TrackingAmazonPrices.Domain;

public class Language
{
    public const string SPANISH = "es";
    public const string ENGLISH = "en";
    public const string ITALIAN = "it";
    public const string FRENCH = "fr";

    public LanguageType LanguageCode { get; set; }

    public Language(string language)
    {
        LanguageCode = GetTypeLanguage(language);
    }

    public Language(LanguageType language)
    {
        LanguageCode = language;
    }

    public Language()
    {
        LanguageCode = LanguageType.English;
    }

    public static LanguageType GetTypeLanguage(string value)
        => value switch
        {
            SPANISH => LanguageType.Spanish,
            ENGLISH => LanguageType.English,
            ITALIAN => LanguageType.Italian,
            FRENCH => LanguageType.French,
            _ => LanguageType.English,
        };
}