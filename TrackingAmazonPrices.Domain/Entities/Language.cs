using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Domain.Entities;

public class Language
{
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
            "es" => LanguageType.Spanish,
            "en" => LanguageType.English,
            _ => LanguageType.English,
        };
}