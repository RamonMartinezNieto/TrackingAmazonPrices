using System.Text.Json.Serialization;
using TrackingAmazonPrices.Domain.Entities;

namespace TrackingAmazonPrices.Infraestructure.Amazon;

public class AmazonListObjects
{
    [JsonPropertyName("desktop_buybox_group_1")]
    public AmazonObject[] AmazonObjects { get; set; }
}
