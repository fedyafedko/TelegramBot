using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using CurrencyAPI.Controllers;
using Telegram.Bot.Types.Enums;


namespace ConvertCurrencyBot
{
    class TelegramBot
    {
        public static ITelegramBotClient _bot = new TelegramBotClient("6046903328:AAFYt_3eYHYekJujM0Lza_5bdIBej4DgneI");
        private static CurrencyController controller;

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                IReplyMarkup keyboardAuth = GetButtonsAuth();
                IReplyMarkup keyboard = GetButtons();
                var message = update.Message;
                if (message.Text == "Сalculator")
                {
                    // Відправляємо повідомлення з текстом "Введіть параметри (наприклад, USD UAH 100):"
                    await botClient.SendTextMessageAsync(message.Chat, "Введіть параметри (наприклад, USD UAH 100):");
                    // Чекаємо на введення тексту з параметрами
                }
                else if (message.Text != null)
                {
                    // розділити текст повідомлення на параметри have, want та amount
                    string[] parameters = message.Text.Split(' ');
                    if (parameters.Length == 3)
                    {
                        string have = parameters[0];
                        string want = parameters[1];
                        int amount = int.Parse(parameters[2]);

                        // викликати контролер та повернути результат в повідомленні
                        var result = await controller.Calculator(have, want, amount);
                        await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: $"Результат: {result}"
                        );
                    }
                }
                //if (message.Text.ToLower() == "/start")
                //{
                //    await botClient.SendTextMessageAsync(message.Chat, "rreeeeeeeeeeehfdhdhfgdggdfgdg");
                //    await botClient.SendTextMessageAsync(message.Chat, text: "Choose options:", replyMarkup: keyboardAuth);
                //}
                //else if (message.Text == "Authorization")
                //{
                //    await botClient.SendTextMessageAsync(message.Chat, "Well, done!!!");
                //    await botClient.SendTextMessageAsync(message.Chat, text: "Choose options:", replyMarkup: keyboard);
                //}
                //else if (message.Text == "Сalculator")
                //{

                //}
            }

        }

        public static IReplyMarkup GetButtons()
        {
            // Створення двох кнопок
            KeyboardButton buttonAllCurrency = new KeyboardButton("All My Currency");
            KeyboardButton buttonUSD = new KeyboardButton("USD");
            KeyboardButton buttonEUR = new KeyboardButton("EUR");
            KeyboardButton buttonGBR = new KeyboardButton("GBR");
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
        public static IReplyMarkup GetButtonsAuth()
        {
            // Створення двох кнопок
            KeyboardButton buttonAuth = new KeyboardButton("Authorization");


            // Створення рядків клавіатури
            KeyboardButton[][] buttons = new KeyboardButton[][]
            {
                new KeyboardButton[] { buttonAuth }

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

