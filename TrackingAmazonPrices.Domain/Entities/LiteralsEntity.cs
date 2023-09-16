using System.Collections.Generic;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Domain.Entities;

public class LiteralsEntity
{
    public LanguageType Language { get; set; }
    public Dictionary<Literals, string> Values { get; set; }

    public LiteralsEntity()
    {
        Values = new();
    }
}
