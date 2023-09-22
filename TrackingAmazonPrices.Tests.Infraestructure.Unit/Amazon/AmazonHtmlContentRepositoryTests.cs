using NSubstitute.ExceptionExtensions;
using System.Net.Http;
using TrackingAmazonPrices.Infraestructure.Amazon;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Amazon;

public class AmazonHtmlContentRepositoryTests
{
    private readonly ILogger<AmazonHtmlContentRepository> _logger = Substitute.For<ILogger<AmazonHtmlContentRepository>>();
    private readonly IHttpClientFactory _factory = Substitute.For<IHttpClientFactory>();

    private readonly IHtmlContentRepository _sut;

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

    [Fact]
    public async Task GetHtmlContent_ReturnStringEmpty_WhenGetIsNotSuccessStatusCode() 
    {
        HttpClient client = Substitute.For<HttpClient>();
        _factory.CreateClient(Arg.Any<string>()).Returns(client);

        //TODO I need an abstraction of HttpClient IHttpClient
        HttpResponseMessage responseMessage = Substitute.For<HttpResponseMessage>();

        client.GetAsync("SOME_CODE").Returns(responseMessage);

        var result = await _sut.GetHtmlContent("SOME_CODE");

        result.Should().BeEmpty();
    }
}
