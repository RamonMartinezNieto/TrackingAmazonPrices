namespace TrackingAmazonPrices.Application.Services;

public interface IHtmlContentRepository
{
    Task<string> GetHtmlContent(string productCode);
}
