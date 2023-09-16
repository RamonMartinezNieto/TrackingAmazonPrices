using TrackingAmazonPrices.Domain.Entities;

namespace TrackingAmazonPrices.Infraestructure;

public class LiteralsServiceSheets : ILiteralsService
{
    private const LanguageType DEFAULT_LANG = LanguageType.English;
    private Dictionary<LanguageType, LiteralsEntity> _allLiterals;
    private readonly ILiteralsClient _literalsClient;

    public LiteralsServiceSheets(
        ILiteralsClient literalsClient)
    {
        _literalsClient = literalsClient;
    }

    public async Task<string> GetAsync(
        LanguageType lang,
        Literals literal)
    {
        _allLiterals ??= await _literalsClient.LoadLiterals();

        if (lang is LanguageType.Default) 
            return _allLiterals[DEFAULT_LANG].Values[literal];

        return _allLiterals[lang].Values[literal];
    }

    public async Task<string> GetAsync(Literals literal)
        => await GetAsync(LanguageType.Default, literal);
}