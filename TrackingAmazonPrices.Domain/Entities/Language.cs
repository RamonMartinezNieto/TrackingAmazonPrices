using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Domain.Entities;

public class Language
{
    public LanguageType LanguageCode { get; set; }

    public Language(string language)
    {
        LanguageCode = GetType(language);
    }

    public Language(LanguageType language)
    {
        LanguageCode = language;
    }

    public Language()
    {
        LanguageCode = LanguageType.English;
    }

    private static LanguageType GetType(string value)
        => value switch
        {
            "es" => LanguageType.Spanish,
            "en" => LanguageType.English,
            _ => LanguageType.English,
        };
}