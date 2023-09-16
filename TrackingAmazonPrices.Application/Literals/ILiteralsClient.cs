using System.Collections.Generic;

namespace TrackingAmazonPrices.Application;

public interface ILiteralsClient
{
    Task<Dictionary<LanguageType, LiteralsEntity>> LoadLiterals();
}