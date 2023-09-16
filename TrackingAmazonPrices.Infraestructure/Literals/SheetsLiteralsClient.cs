using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.IO;
using TrackingAmazonPrices.Domain.Entities;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.GetRequest;

namespace TrackingAmazonPrices.Infraestructure;

public class SheetsLiteralsClient : ILiteralsClient
{
    private const string START_CELL = "A";
    private const string END_CELL = "C";

    private SheetsService _sheetsService;
    private readonly ILogger<LiteralsServiceSheets> _logger;
    private readonly SheetConfiguration _config;

    public SheetsLiteralsClient(
          ILogger<LiteralsServiceSheets> logger,
          IOptions<SheetConfiguration> options)
    {
        _logger = logger;
        _config = options.Value;
    }

    public async Task<Dictionary<LanguageType, LiteralsEntity>> LoadLiterals()
    {
        if (!await EnsureInitializedClientAsync())
            throw new InvalidOperationException("Connection not established");

        var literals = await GetLiteralsFromSheetAsync();
        return ToDictionaryModel(literals);
    }

    private async Task<IList<IList<object>>> GetLiteralsFromSheetAsync()
    {
        var range = $"!{START_CELL}:{END_CELL}";
        var request = _sheetsService.Spreadsheets.Values.Get(_config.SheetId, range);
        request.MajorDimension = MajorDimensionEnum.COLUMNS;
        ValueRange response = await request.ExecuteAsync();
        return response.Values;
    }

    private async Task<bool> EnsureInitializedClientAsync()
    {
        if (_sheetsService != null)
            return true;

        _logger.LogInformation("Initialicing google sheets client");

        try
        {
            using var stream = new FileStream(_config.PathCredentials, FileMode.Open, FileAccess.Read);
            var credential = (await GoogleCredential.FromStreamAsync(stream, CancellationToken.None))
                .CreateScoped(SheetsService.Scope.Spreadsheets);

            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Imposible connect with google sheets {ErrMessage}", ex.Message);
            return false;
        }

        return true;
    }

    public static Dictionary<LanguageType, LiteralsEntity> ToDictionaryModel(IList<IList<object>> valueRange)
    {
        Dictionary<LanguageType, LiteralsEntity> values = new();

        var keys = valueRange[0];
        var literalsValues = valueRange.Take(1..);

        foreach (var items in literalsValues)
        {
            LiteralsEntity literalEntity = new();

            for (int i = 0; i < items.Count; i++)
            {
                if (i == 0)
                    literalEntity.Language = Language.GetTypeLanguage((string)items[i]);
                else
                {
                    string key = (string)keys[i];
                    literalEntity.Values.Add((Literals)Enum.Parse(typeof(Literals), key), (string)items[i]);
                }
            }
            values.Add(literalEntity.Language, literalEntity);
        }

        return values;
    }
}