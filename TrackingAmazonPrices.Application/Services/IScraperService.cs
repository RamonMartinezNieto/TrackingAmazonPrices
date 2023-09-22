namespace TrackingAmazonPrices.Application.Services;

public interface IScraperService<T>
{
    Task<AmazonObject> GetObject(Uri url);
}
