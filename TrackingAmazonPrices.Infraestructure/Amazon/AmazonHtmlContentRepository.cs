using System.Net.Http;
using TrackingAmazonPrices.Application.Services;

namespace TrackingAmazonPrices.Infraestructure.Amazon;

public class AmazonHtmlContentRepository : IHtmlContentRepository
{
    private readonly ILogger<AmazonHtmlContentRepository> _logger;
    private readonly IHttpClientFactory _factory;

    public AmazonHtmlContentRepository(
        ILogger<AmazonHtmlContentRepository> logger,
        IHttpClientFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    public async Task<string> GetHtmlContent(string productCode)
    {
        try {
            using HttpClient client = _factory.CreateClient("AmazonClient");

            var result = await client.GetAsync(productCode);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsStringAsync();
            }

            _logger.LogWarning("Fail getting content of the {prodcut}", productCode);

            return string.Empty;
        }
        catch (Exception ex) 
        {
            _logger.LogError("Imposible to get html contento from amazon with the {productCode}", productCode);
            _logger.LogError("{Exception}", ex.ToString());

            throw new InvalidOperationException($"Fail getting html content from amazon.es ProductCode {productCode}", ex);
        }
    }
}
