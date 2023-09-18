using FluentAssertions;
using TrackingAmazonPrices.Domain;
using Xunit;

namespace TrackingAmazonPrices.Tests.Domain.Unit.Entities;

public class LanguageTest
{
    [Fact]
    public void InstantiateLanguage_ReturnEnglish_VoidLanguage()
    {
        Language language = new();

        language.LanguageCode.Should().Be(LanguageType.English);
    }

    [Theory]
    [InlineData(LanguageType.Spanish)]
    [InlineData(LanguageType.English)]
    [InlineData(LanguageType.Italian)]
    [InlineData(LanguageType.French)]
    public void InstantiateLanguage_ReturnSpanish_TypeConstructor(
        LanguageType inputAndExpected)
    {
        Language language = new(inputAndExpected);

        language.LanguageCode.Should().Be(inputAndExpected);
    }

    [Theory]
    [InlineData("es", LanguageType.Spanish)]
    [InlineData("en", LanguageType.English)]
    [InlineData("fi", LanguageType.English)]
    [InlineData("it", LanguageType.Italian)]
    [InlineData("fr", LanguageType.French)]
    public void InstantiateLanguage_ReturnSpanish_StringConstructorEn(
        string inputBigrama,
        LanguageType expectedLanguage)
    {
        Language language = new(inputBigrama);

        language.LanguageCode.Should().Be(expectedLanguage);
    }
}