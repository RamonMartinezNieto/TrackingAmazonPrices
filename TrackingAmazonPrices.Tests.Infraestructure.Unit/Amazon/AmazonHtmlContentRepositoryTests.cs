using NSubstitute.ExceptionExtensions;
using System.Net.Http;
using TrackingAmazonPrices.Infraestructure.Amazon;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Amazon;

public class AmazonHtmlContentRepositoryTests
{
    private readonly ILogger<AmazonHtmlContentRepository> _logger = Substitute.For<ILogger<AmazonHtmlContentRepository>>();
    private readonly IHttpClientFactory _factory = Substitute.For<IHttpClientFactory>();

    IHtmlContentRepository _sut;

    public AmazonHtmlContentRepositoryTests()
    {
        _sut = new AmazonHtmlContentRepository(_logger, _factory);
    }

    [Fact]
    public async Task GetHtmlContent_ThrowInvalidOperationException_WhenClientIsNotConfigured() 
    {
        _factory.CreateClient(Arg.Any<string>()).Returns(x => throw new InvalidOperationException());

        Func<Task> act = async () => await _sut.GetHtmlContent("SOME_CODE");

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Fail getting html content from amazon.es ProductCode SOME_CODE");
    }

}
