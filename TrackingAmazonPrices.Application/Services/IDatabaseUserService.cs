namespace TrackingAmazonPrices.Application.Services;

public interface IDatabaseUserService
{
    Task<bool> SaveUserAsync(User user);

    Task<LanguageType> GetLanguage(long id);

    Task<bool> DeleteUser(long id);

    Task<bool> UserExists(long id);
}