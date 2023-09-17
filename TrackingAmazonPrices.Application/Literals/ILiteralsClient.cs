using System.Collections.Generic;
using TrackingAmazonPrices.Domain;

namespace TrackingAmazonPrices.Application;

public interface ILiteralsClient
{
    Task<Dictionary<LanguageType, LiteralsEntity>> LoadLiterals();
}