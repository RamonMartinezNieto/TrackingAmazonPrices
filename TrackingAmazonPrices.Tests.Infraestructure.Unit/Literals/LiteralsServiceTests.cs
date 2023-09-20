using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Domain.Entities;
using TrackingAmazonPrices.Infraestructure;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.LiteralsTest;

public class LiteralsServiceTests
{
    private readonly ILiteralsClient _literalsClient = Substitute.For<ILiteralsClient>();
    private readonly ILiteralsService _sut;

    public LiteralsServiceTests()
    {
        _sut = new LiteralsServiceSheets(_literalsClient);

        var mockLiterals = GetMockLiterals();
        _literalsClient.LoadLiterals().Returns(mockLiterals);
    }

    [Fact]
    public async Task GetLiterals_ReturnWelcome_WhenLanguageIsEnglish()
    {
        var result = await _sut.GetAsync(LanguageType.English, Literals.Welcome);

        result.Should().NotBeNull();
        result.Should().Be("Welcome");
    }

    [Fact]
    public async Task GetLiterals_ReturnBienvenido_WhenLanguageIsSpanis()
    {
        var result = await _sut.GetAsync(LanguageType.Spanish, Literals.Welcome);

        result.Should().NotBeNull();
        result.Should().Be("Bienvenido");
    }

    [Fact]
    public async Task GetLiterals_ReturnWelcome_WhenLanguageIsNothing()
    {
        var result = await _sut.GetAsync(Literals.Welcome);

        result.Should().NotBeNull();
        result.Should().Be("Welcome");
    }

    [Fact]
    public async Task GetLiterals_ReturnSelectLang_WhenLanguageIsEnglish()
    {
        var result = await _sut.GetAsync(LanguageType.English, Literals.SelectLan);

        result.Should().NotBeNull();
        result.Should().Be("Select a language");
    }

    [Fact]
    public async Task GetLiterals_ReturnSeleccionaLang_WhenLanguageIsSpanis()
    {
        var result = await _sut.GetAsync(LanguageType.Spanish, Literals.SelectLan);

        result.Should().NotBeNull();
        result.Should().Be("Selecciona un idioma");
    }

    private static Dictionary<LanguageType, LiteralsEntity> GetMockLiterals()
    {
        Dictionary<LanguageType, LiteralsEntity> literals = new()
        {
            {
                LanguageType.Spanish,
                new LiteralsEntity()
                {
                    Language = LanguageType.Spanish,
                    Values = new()
            {
                { Literals.Welcome, "Bienvenido" },
                { Literals.SelectLan, "Selecciona un idioma" }
            }
                }
            },

            {
                LanguageType.English,
                new LiteralsEntity()
                {
                    Language = LanguageType.English,
                    Values = new()
            {
                { Literals.Welcome, "Welcome" },
                { Literals.SelectLan, "Select a language" }
            }
                }
            }
        };

        return literals;
    }
}