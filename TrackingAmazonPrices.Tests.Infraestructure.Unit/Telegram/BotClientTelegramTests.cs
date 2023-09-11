namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Telegram;

public class BotClientTelegramTEsts
{
    [Fact]
    public void InstantiateBotClientTelegram_ShoulBeBotClientTelegram()
    {
        string token = "askdjhs76fd87sfgdhasjdf";
        var botConfig = Options.Create(new BotConfig { Token = token });

        BotClientTelegram _sut = new(botConfig);

        _sut.BotClient.Should().NotBeNull();
        _sut.BotClient.Should().BeAssignableTo<TelegramBotClient>();
    }
}