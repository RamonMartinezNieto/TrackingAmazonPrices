using TrackingAmazonPrices.Domain;

namespace TrackingAmazonPrices.Application;

public interface ILiteralsService
{
    Task<string> GetAsync(LanguageType lang, Literals literal);

    Task<string> GetAsync(Literals literal);
}