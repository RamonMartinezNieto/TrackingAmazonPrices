using NSubstitute.ExceptionExtensions;
using TrackingAmazonPrices.Domain.Entities;
using TrackingAmazonPrices.Infraestructure.Amazon;
using TrackingAmazonPrices.Tests.Infraestructure.Unit.Mocks;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Amazon;

public class AmazonScraperServiceTests
{
    private readonly ILogger<AmazonScraperService> _logger = Substitute.For<ILogger<AmazonScraperService>>();
    private readonly IHtmlContentRepository _contentRepository = Substitute.For<IHtmlContentRepository>();

    private readonly AmazonScraperService _sut;

    public AmazonScraperServiceTests() 
    {
        _sut = new(_logger, _contentRepository);
    }

    [Fact]
    public async Task GetObject_ReturnEmpty_NotPossibleExctractProductCode() 
    {
        AmazonObject exceptected = new(); 

        Uri url = new ("https://someurl.com");
        
        AmazonObject result = await _sut.GetObject(url);

        result.Should().BeEquivalentTo(exceptected);
    }    
    
    [Fact]
    public async Task GetObject_ThrowException_WhenHtmlContentFails() 
    {
        Uri url = new ("https://amazon.com/dp/21343456");
        
        _contentRepository
            .GetHtmlContent("21343456")
            .ThrowsAsync<InvalidOperationException>();
        
        Func<Task> act = async () => await _sut.GetObject(url);

        await act.Should()
            .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetObject_VoidObject_WhenStatusCodeIsNotSuccess() 
    {
        AmazonObject exceptected = new();

        Uri url = new ("https://amazon.com/dp/21343456");
        
        _contentRepository
            .GetHtmlContent("21343456")
            .Returns(string.Empty);
        
        AmazonObject result = await _sut.GetObject(url);

        result.Should().BeEquivalentTo(exceptected);
    }

    [Fact]
    public async Task GetObject_CompleteObject_WhenSucces() 
    {
        Uri url = new ("https://amazon.es/dp/21343456");
        
        _contentRepository
            .GetHtmlContent("21343456")
            .Returns(MockHtmlContentAmazon.GetHtmlContent());
        
        AmazonObject result = await _sut.GetObject(url);

        result.Ansi.Should().Be("21343456");
        result.PriceAmount.Should().Be(37.95f);
        result.CurrencySymbol.Should().Be("€");
        result.TopLevelDomain.Should().Be("es");
        result.Description.Should().Be("El programador pragmático. Edición especial: Viaje a la maestría (TÍTULOS ESPECIALES) : Thomas, David, Hunt, Andrew: Amazon.es: Libros");
    }

    [Fact]
    public async Task GetObject_AmazonObjectWithoutTopLevelDomain_WhenUriDoesntHaveTopLevelDomain() 
    {
        Uri url = new ("https://amazones/dp/21343456");
        
        _contentRepository
            .GetHtmlContent("21343456")
            .Returns(MockHtmlContentAmazon.GetHtmlContent());
        
        AmazonObject result = await _sut.GetObject(url);

        result.Ansi.Should().Be("21343456");
        result.PriceAmount.Should().Be(37.95f);
        result.CurrencySymbol.Should().Be("€");
        result.Description.Should().Be("El programador pragmático. Edición especial: Viaje a la maestría (TÍTULOS ESPECIALES) : Thomas, David, Hunt, Andrew: Amazon.es: Libros");
    }

    [Fact]
    public async Task GetObject_AmazonObjectWithoutDescriptiont_WhenTereIsntDescription() 
    {
        Uri url = new ("https://amazon.es/dp/21343456");
        
        _contentRepository
            .GetHtmlContent("21343456")
            .Returns(MockHtmlContentAmazon.GetHtmlContentWithoutDescription());
        
        AmazonObject result = await _sut.GetObject(url);

        result.Ansi.Should().Be("21343456");
        result.PriceAmount.Should().Be(37.95f);
        result.CurrencySymbol.Should().Be("€");
        result.TopLevelDomain.Should().Be("es");
        result.Description.Should().Be(null);
    }
}
