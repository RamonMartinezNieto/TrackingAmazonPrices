using FluentAssertions;
using TrackingAmazonPrices.Domain;
using Xunit;

namespace TrackingAmazonPrices.Tests.Domain.Unit.Entities;

public class LanguageTest
{
    [Fact]
    public void InstantiateLanguage_ReturnEngliss_VoidLanguage()
    {
        Language language = new();

        language.LanguageCode.Should().Be(LanguageType.English);
    }

    [Fact]
    public void InstantiateLanguage_ReturnSpanish_TypeConstructor()
    {
        Language language = new(LanguageType.Spanish);

        language.LanguageCode.Should().Be(LanguageType.Spanish);
    }

    [Fact]
    public void InstantiateLanguage_ReturnSpanish_StringConstructorEn()
    {
        Language language = new("en");

        language.LanguageCode.Should().Be(LanguageType.English);
    }

    [Fact]
    public void InstantiateLanguage_ReturnSpanish_StringConstructorEs()
    {
        Language language = new("es");

        language.LanguageCode.Should().Be(LanguageType.Spanish);
    }
    [Fact]
    public void InstantiateLanguage_ReturnSpanish_StringConstructorDefault()
    {
        Language language = new("fi");

        language.LanguageCode.Should().Be(LanguageType.English);
    }
}