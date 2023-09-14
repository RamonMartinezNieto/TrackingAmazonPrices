using TrackingAmazonPrices.Domain.Entities;

namespace TrackingAmazonPrices.Application.Services;

public interface IDatabaseUserService
{
    Task<bool> SaveUserAsync(User user);
}
