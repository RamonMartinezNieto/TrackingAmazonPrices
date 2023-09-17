using TrackingAmazonPrices.Domain;

namespace TrackingAmazonPrices.Application.Services;

public interface IDatabaseUserService
{
    Task<bool> SaveUserAsync(User user);

    Task<LanguageType> GetLanguage(long id);
}