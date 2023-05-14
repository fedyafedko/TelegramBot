using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using Currency.BLL.Currency.API.Common.DTO;
using TelegramBot;
using System.Text;
using CurrencyDAL.EF;

namespace ConvertCurrencyBot
{
    class TelegramBot
    {
        public static ITelegramBotClient _bot = new TelegramBotClient("6046903328:AAFYt_3eYHYekJujM0Lza_5bdIBej4DgneI");
        private static Dictionary<long, string> userState = new Dictionary<long, string>();
        private static ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                IReplyMarkup keyboard = Keyboard.GetButtons();
                IReplyMarkup keyboardUp = Keyboard.GetButtonsUpdate();
                var message = update.Message;

                if (userState.ContainsKey(message!.Chat.Id) && userState[message.Chat.Id] == "Calculator")
                {
                    string[] parameters = message.Text.Split(' ');
                    if (parameters.Length == 3)
                    {
                        string have = parameters[0];
                        string want = parameters[1];
                        int amount = int.Parse(parameters[2]);
                        var httpClient = new HttpClient();
                        var url = $"https://localhost:7181/Currencies/Calrulator?have={have}&want={want}&amount={amount}";
                        var response = await httpClient.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<CurrencyDTO>(responseContent);
                        await botClient.SendTextMessageAsync(message.Chat, text: $"💵 {amount} {result?.old_currency} = {result?.new_amount} {result?.new_currency}");
                    }

                    userState.Remove(message.Chat.Id);
                    await botClient.SendTextMessageAsync(message.Chat, "Calculator mode has been exited.", replyMarkup: keyboard);
                }
                else if (userState.ContainsKey(message!.Chat.Id) && userState[message.Chat.Id] == "Update My Currency")
                {
                    string[] parameters = message.Text.Split(' ');
                    string have = parameters[0];
                    string want = parameters[1];
                    int amount = int.Parse(parameters[2]);
                    int resultAmount = int.Parse(parameters[3]);

                    var currency = await _applicationDbContext.Currencies.FindAsync(have);
                    if (currency != null)
                    {
                        var data = new
                        {
                            new_currency = want,
                            old_amount = amount,
                            new_amount = resultAmount
                        };

                        var httpClient = new HttpClient();
                        var url = $"https://localhost:7181/Currencies/{have}";
                        var jsonContent = JsonConvert.SerializeObject(data);
                        var requestContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                        var response = await httpClient.PutAsync(url, requestContent);
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<CurrencyDTO>(responseContent);

                        await botClient.SendTextMessageAsync(message.Chat, text: $"💰 {have}/{result.new_currency}: {result.new_amount} \n📅 {result.date.Date.ToString("dd-MM-yyyy")}");
                        await botClient.SendTextMessageAsync(message.Chat, text: "Fantastic! You've just updated your currency🫶", replyMarkup: keyboard);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat, text: "Sorry, we could not find that currency in our database.");
                    }
                }
                else if (userState.ContainsKey(message!.Chat.Id) && userState[message.Chat.Id] == "Delete My Currency")
                {
                    var have = message.Text;
                    var currency = await _applicationDbContext.Currencies.FindAsync(have);
                    if (currency == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat, text: "Sorry, we could not find that currency in our database.");
                    }
                    else
                    {
                        var httpClient = new HttpClient();
                        var url = $"https://localhost:7181/Currencies/{have}";
                        var response = await httpClient.DeleteAsync(url);
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<CurrencyDTO>(responseContent);
                        userState.Remove(message.Chat.Id);
                        await botClient.SendTextMessageAsync(message.Chat, text: "Fantastic! You've just deleted your currency🫶", replyMarkup: keyboard);
                    }
                }
                else
                {
                    if (message.Text.ToLower() == "/start")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Hello!!!👋\nI am your telegram bot for currency exchange. I was created to help you track currency exchange rates and learn up-to-date information about the currency market.\r\nI am always in touch and ready to answer your questions and offer the best currency exchange options.\r\nThank you for being with us!🤑");
                        await botClient.SendTextMessageAsync(message.Chat, text: "Choose options:", replyMarkup: keyboard);
                    }
                    else if (message.Text == "Сalculator")
                    {
                        userState[message.Chat.Id] = "Calculator";
                        await botClient.SendTextMessageAsync(message.Chat, "Enter parameters (for example, USD UAH 100):");
                    }
                    else if (message.Text.Length == 3)
                    {
                        var have = message.Text;
                        var currency = await _applicationDbContext.Currencies.FindAsync(have);
                        if (currency == null)
                        {
                            var data = new { have = have };
                            var httpClient = new HttpClient();
                            var url = $"https://localhost:7181/Currencies?have={have}";
                            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                            var response = await httpClient.PostAsync(url, content);
                            response.EnsureSuccessStatusCode();
                            var responseContent = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<CurrencyDTO>(responseContent);
                            await botClient.SendTextMessageAsync(message.Chat, text: $"💰 {result.old_currency}/{result.new_currency}: {result.new_amount} \n📅 {result.date.Date.ToString("dd-MM-yyyy")}");
                        }
                        else
                        {
                            var httpClient = new HttpClient();
                            var url = $"https://localhost:7181/Currencies/{message.Text}";
                            var response = await httpClient.GetAsync(url);
                            response.EnsureSuccessStatusCode();
                            var responseContent = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<CurrencyDTO>(responseContent);
                            await botClient.SendTextMessageAsync(message.Chat, text: $"💰 {result.old_currency}/{result.new_currency}: {result.new_amount} \n📅 {result.date.Date.ToString("dd-MM-yyyy")}");
                        }
                    }
                    else if (message.Text == "All My Currency")
                    {
                        var httpClient = new HttpClient();
                        var url = "https://localhost:7181/Currencies";
                        var response = await httpClient.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        var responseContent = await response.Content.ReadAsStringAsync();
                        List<CurrencyDTO> resultList = JsonConvert.DeserializeObject<List<CurrencyDTO>>(responseContent)!;
                        string messageText = "Currency rates:\n";
                        foreach (CurrencyDTO currency in resultList)
                        {
                            messageText += $"💰{currency.old_currency}/{currency.new_currency}: {currency.new_amount}\n📅 {currency.date.Date.ToString("dd-MM-yyyy")}\n\n";
                        }
                        await botClient.SendTextMessageAsync(message.Chat, text: $"{messageText}");
                        await botClient.SendTextMessageAsync(message.Chat, text: "💵If you wish to change your currency, please select one of the following options:", replyMarkup: keyboardUp);
                    }
                    else if (message.Text == "Update My Currency")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, text: "💰If you want to update currency enter parameters (for example, USD UAH 100)");
                        userState[message.Chat.Id] = "Update My Currency";
                    }
                    else if (message.Text == "Delete My Currency")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, text: "💰If you want to delete currency enter parameters (for example, USD):");
                        userState[message.Chat.Id] = "Delete My Currency";
                    }
                    else if (message.Text == "Add Currency")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, text: "💰If you want to add currency enter parameters (for example, USD):");
                        userState[message.Chat.Id] = "Add Currency";
                    }
                    else if (message.Text == "Exit")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, text: "Choose options:", replyMarkup: keyboard);
                    }
                }
            }
        }
        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}

