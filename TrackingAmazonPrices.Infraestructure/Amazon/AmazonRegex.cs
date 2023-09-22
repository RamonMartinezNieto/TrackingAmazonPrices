using System.Text.RegularExpressions;

namespace TrackingAmazonPrices.Infraestructure.Amazon;

public partial class AmazonRegex
{
    protected AmazonRegex() { }
    
    [GeneratedRegex(
        "<meta\\sname=\"description\"\\scontent=\"([^\"]*)",
        RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    internal static partial Regex AmazonMetaDescription();

    [GeneratedRegex(
        "<div\\sclass=\"a-section aok-hidden twister-plus-buying-options-price-data\">\\s*({.*?})\\s*</div>",
        RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    internal static partial Regex AmazonPriceDataObject();

    [GeneratedRegex(
        "\\/dp\\/(\\w+)",
        RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    internal static partial Regex AmazonAnsi();
}