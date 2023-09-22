namespace TrackingAmazonPrices.Domain.Entities;

public class AmazonObject
{
    public float PriceAmount { get; set; }

    public string CurrencySymbol { get; set; }

    public string Description { get; set; }

    public string TopLevelDomain { get; set; }

    public string Ansi { get; set; }

    public string GetDisplayPrice()
        => $"{PriceAmount} {CurrencySymbol}";
}
