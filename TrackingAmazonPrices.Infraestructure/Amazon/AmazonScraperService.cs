using System.Text.Json;
using System.Text.RegularExpressions;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Entities;

namespace TrackingAmazonPrices.Infraestructure.Amazon;

public class AmazonScraperService : AmazonRegex, IScraperService<AmazonObject>
{
    private readonly ILogger<AmazonScraperService> _logger;
    private readonly IHtmlContentRepository _contentRepository;

    public AmazonScraperService(
        ILogger<AmazonScraperService> logger,
        IHtmlContentRepository contentRepository)
    {
        _logger = logger;
        _contentRepository = contentRepository;
    }

    public async Task<AmazonObject> GetObject(Uri url)
    {
        AmazonObject amazonObj = new();
        
        _logger.LogInformation("Getting object");

        if (TryGetProductCode(url.ToString(), out string productCode))
        {
            _logger.LogInformation("object product code {code}", productCode);

            string htmlContent = await _contentRepository.GetHtmlContent(productCode);

            if (TryGetProdudct(htmlContent, out amazonObj)) 
            {
                amazonObj.Ansi = productCode;

                if (TryGetDescription(htmlContent, out string title))
                    amazonObj.Description = title;

                if (TryGetTopLevelDomain(url, out string topLevelDomain))
                    amazonObj.TopLevelDomain = topLevelDomain;
            }
        }
        return amazonObj;
    }

    private static bool TryGetTopLevelDomain(Uri url, out string topLevelDomain)
    {
        topLevelDomain = string.Empty;

        string[] domainParts = url.Host.Split('.');
        if (domainParts.Length >= 2)
        {
            topLevelDomain = domainParts[^1];
            return true;
        }

        return false;
    }

    private static bool TryGetProdudct(string htmlContent, out AmazonObject product)
    {
        Match match = AmazonPriceDataObject().Match(htmlContent);

        if (match.Success && match.Groups?.Count > 1)
        {
            AmazonListObjects amazonObjects = JsonSerializer.Deserialize<AmazonListObjects>(
                match.Groups[1].Value,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            if (amazonObjects?.AmazonObjects.Length > 0)
            {
                product = amazonObjects.AmazonObjects[0];
                return true;
            }
        }

        product = new();
        return false;
    }

    private static bool TryGetDescription(string htmlContent, out string description)
    {
        Match match = AmazonMetaDescription().Match(htmlContent);

        if (match?.Groups?.Count > 1)
        {
            description = match.Groups[1].Value;
            return true;
        }
        description = string.Empty;
        return false;
    }

    private static bool TryGetProductCode(string url, out string code)
    {
        code = string.Empty;
        Match match = AmazonAnsi().Match(url);

        if (match?.Groups?.Count > 1)
        {
            code = match.Groups[1].Value;
            return true;
        }
        return false;
    }
}