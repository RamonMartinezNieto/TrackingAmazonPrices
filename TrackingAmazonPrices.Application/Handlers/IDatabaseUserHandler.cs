using TrackingAmazonPrices.Domain.Entities;

namespace TrackingAmazonPrices.Application.Handlers;

public interface IDatabaseUserHandler
{
    Task<bool> SaveUserAsync(User user);
}
