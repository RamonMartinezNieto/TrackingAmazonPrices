using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Domain.Entities;

public class Language
{
    public const string SPANISH = "es";
    public const string ENGLISH = "en";
    public const string ITALIAN = "it";
    public const string FRENCH = "fr";

    public LanguageType LanguageCode { get; set; }

    private Language(string language)
        => LanguageCode = GetTypeLanguage(language);

    private Language(LanguageType language)
        => LanguageCode = language;

    public Language()
        => LanguageCode = LanguageType.English;

    public static implicit operator Language(string lang) => new (lang);

    public static implicit operator Language(LanguageType lang) => new (lang);

    public static implicit operator LanguageType(Language lang) => lang.LanguageCode;

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