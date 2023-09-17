using Telegram.Bot.Types.ReplyMarkups;
using TrackingAmazonPrices.Domain.Enums;
using static TrackingAmazonPrices.Domain.Language;

namespace TrackingAmazonPrices.Infraestructure.Telegram;

public static class UtilsTelegramMessage
{
    public static InlineKeyboardMarkup CreateMenu(List<string[,]> data)
    {
        List<InlineKeyboardButton[]> menuRows = new();

        for (int i = 0; i < data.Count; i++)
        {
            List<InlineKeyboardButton> buttons = new();

            var row = data[i];

            int lenght = row.GetLength(0);
            for (int z = 0; z < lenght; z++)
            {
                buttons.Add(new(row[z, 0]) { 
                    CallbackData = row[z, 1],
                });
            }

            menuRows.Add(buttons.ToArray());
        }

        return new InlineKeyboardMarkup(menuRows);
    }

    private const string LANGUAGE_CALLBACK = "/callbackLanguage_";
    public static List<string[,]> GetMenuLanguageRows() 
        => new()
        {
            new string[4, 2] {
                { "ES " + TelegramEmojis.ES_FLAG, LANGUAGE_CALLBACK + SPANISH},
                { "EN " + TelegramEmojis.GB_FLAG, LANGUAGE_CALLBACK + ENGLISH },
                { "IT " + TelegramEmojis.IT_FLAG, LANGUAGE_CALLBACK + ITALIAN },
                { "FR " + TelegramEmojis.FR_FLAG, LANGUAGE_CALLBACK + FRENCH } }
        };
}