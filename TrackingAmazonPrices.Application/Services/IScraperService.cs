namespace TrackingAmazonPrices.Application.Services;

public interface IScraperService<T>
{
    Task<T> GetObject(Uri url);
}
