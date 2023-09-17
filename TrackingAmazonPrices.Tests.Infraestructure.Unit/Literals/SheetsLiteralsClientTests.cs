using Google.Apis.Sheets.v4.Data;
using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Domain.Entities;
using TrackingAmazonPrices.Domain;
using TrackingAmazonPrices.Infraestructure;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit;

public class SheetsLiteralsClientTests
{
    private readonly ILogger<LiteralsServiceSheets> _logger = Substitute.For<ILogger<LiteralsServiceSheets>>();
    private readonly IOptions<SheetConfiguration> _options = Substitute.For<IOptions<SheetConfiguration>>();

    [Fact]
    public async void LoadLiterals_ShouldThrowException_WhenConnectionFail()
    {

        var sheetConfiguration = new SheetConfiguration
        {
            SheetId = "someSheetId",
            PathCredentials = "somePath"
        };

        _options.Value.Returns(sheetConfiguration);

        ILiteralsClient _sut = new SheetsLiteralsClient(_logger, _options);

        Func<Task> act = async () => await _sut.LoadLiterals();

        await act.Should()
                  .ThrowAsync<InvalidOperationException>()
                  .WithMessage("Connection not established");
    }

    [Fact]
    public void ToDictionaryModel_ReturnsDictionary_WhenSendValueRange()
    {

        var simulatedValueRange = new ValueRange
        {
            Values = new List<IList<object>>
            {
                new List<object> { "Key", "Welcome", "SelectLan" },
                new List<object> { "es", "Bienvenido a Tracking Amazon Prices", "Selecciona un idioma" },
                new List<object> { "en", "Welcome to Tracking Amazon Prices", "Select a language" }
            }
        };


        Dictionary<LanguageType, LiteralsEntity> result = SheetsLiteralsClient.ToDictionaryModel(simulatedValueRange.Values);

        result.Should().HaveCount(2);
        result.Keys.Should().HaveCount(2);
        result[LanguageType.Spanish].Language.Should().Be(LanguageType.Spanish);
        result[LanguageType.Spanish].Values.Should().HaveCount(2);
        result[LanguageType.Spanish].Values[Literals.Welcome].Should().Be("Bienvenido a Tracking Amazon Prices");
        result[LanguageType.Spanish].Values[Literals.SelectLan].Should().Be("Selecciona un idioma");

        result[LanguageType.English].Language.Should().Be(LanguageType.English);
        result[LanguageType.English].Values.Should().HaveCount(2);
        result[LanguageType.English].Values[Literals.Welcome].Should().Be("Welcome to Tracking Amazon Prices");
        result[LanguageType.English].Values[Literals.SelectLan].Should().Be("Select a language");
    }
}
