using Telegram.Bot.Types.ReplyMarkups;

namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Telegram;

public class UtilsTelegramMessageTests
{
    [Fact]
    public void CreateMenuMultipleDimensions_ReturnMenu_WhenParamsAreCorrect()
    {
        List<string[,]> menuRows = new()
        {
            new string[2, 2] { { "ES", "ESP" }, { "EN", "UUEE" } },
            new string[3, 2] { { "ES2", "ESP2" }, { "EN2", "UUEE2" }, { "EN22", "UUEE22" } }
        };

        var result = UtilsTelegramMessage.CreateMenu(menuRows);

        result.Should().NotBeNull();
        result.Should().BeOfType<InlineKeyboardMarkup>();

        result.InlineKeyboard.Should().HaveCount(2);
        result.InlineKeyboard.First().Should().HaveCount(2);
        result.InlineKeyboard.ElementAt(1).Should().HaveCount(3);

        result.InlineKeyboard.First().ElementAt(0).Text.Should().Be("ES");
        result.InlineKeyboard.First().ElementAt(0).CallbackData.Should().Be("ESP");

        result.InlineKeyboard.First().ElementAt(1).Text.Should().Be("EN");
        result.InlineKeyboard.First().ElementAt(1).CallbackData.Should().Be("UUEE");

        result.InlineKeyboard.ElementAt(1).ElementAt(2).Text.Should().Be("EN22");
        result.InlineKeyboard.ElementAt(1).ElementAt(2).CallbackData.Should().Be("UUEE22");
    }
}