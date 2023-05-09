using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using CurrencyAPI.Controllers;
using Newtonsoft.Json;
using Currency.BLL.Currency.API.Common.DTO;
using System.Net.Http.Headers;
using CurrencyDAL.Entities;

namespace ConvertCurrencyBot
{
    class TelegramBot
    {
        public static ITelegramBotClient _bot = new TelegramBotClient("6046903328:AAFYt_3eYHYekJujM0Lza_5bdIBej4DgneI");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                IReplyMarkup keyboard = GetButtons();
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Hello!!!\U0001F44B\nI am your telegram bot for currency exchange. I was created to help you track currency exchange rates and learn up-to-date information about the currency market.\r\nI am always in touch and ready to answer your questions and offer the best currency exchange options.\r\nThank you for being with us!\U0001F4B0");
                    await botClient.SendTextMessageAsync(message.Chat, text: "Choose options:", replyMarkup: keyboard);
                }
                if (message.Text == "Сalculator")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Enter parameters (for example, USD UAH 100):");
                    string[] parameters = message.Text.Split(' ');
                    if (parameters.Length == 3)
                    {
                        string have = parameters[0];
                        string want = parameters[1];
                        int amount = int.Parse(parameters[2]);
                        var httpClient = new HttpClient();
                        //var request = message.Text;
                        var url = $"https://localhost:7181/Currencies/Calrulator?have={have}&want={want}&amount={amount}";
                        var response = await httpClient.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<CurrencyDTO>(responseContent);
                        await botClient.SendTextMessageAsync(message.Chat, text: $"{amount} {result.old_currency} = {result.new_amount} {result.new_currency}");
                    }
                }
                else if (message.Text.Length == 3)
                {
                    var httpClient = new HttpClient();
                    var request = message.Text;
                    var url = $"https://localhost:7181/Currencies/{request}";
                    var response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<CurrencyDTO>(responseContent);
                    await botClient.SendTextMessageAsync(message.Chat, text: $"{result.old_currency} {result.new_currency} {result.new_amount} \n{result.date}");
                }
            }

        }
        public static IReplyMarkup GetButtons()
        {
            // Створення двох кнопок
            KeyboardButton buttonAllCurrency = new KeyboardButton("All My Currency");
            KeyboardButton buttonUSD = new KeyboardButton("USD");
            KeyboardButton buttonEUR = new KeyboardButton("EUR");
            KeyboardButton buttonGBR = new KeyboardButton("GBP");
            KeyboardButton buttonPLN = new KeyboardButton("PLN");
            KeyboardButton buttonCalculator = new KeyboardButton("Сalculator");


            // Створення рядків клавіатури
            KeyboardButton[][] buttons = new KeyboardButton[][]
            {
                new KeyboardButton[] { buttonAllCurrency},
                new KeyboardButton[] { buttonUSD, buttonEUR, buttonGBR, buttonPLN },
                new KeyboardButton[] { buttonCalculator}
            };

            // Створення об'єкту клавіатури
            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(buttons);

            // Повертаємо об'єкт клавіатури через IReplyMarkup
            return keyboard;
        }
        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}

