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
                    int resultAmount = int.Parse(parameters[2]);

                    var currency = await _applicationDbContext.Currencies.FindAsync(have);
                    if (currency != null)
                    {
                        var data = new
                        {
                            new_currency = want,
                            old_amount = 1,
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
                        userState.Remove(message.Chat.Id);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat, text: "😔Sorry, we could not find that currency in our database.");
                        userState.Remove(message.Chat.Id);
                    }
                }
                else if (userState.ContainsKey(message!.Chat.Id) && userState[message.Chat.Id] == "Delete My Currency")
                {
                    var have = message.Text;
                    var currency = await _applicationDbContext.Currencies.FindAsync(have);
                    if (currency == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat, text: "😔Sorry, we could not find that currency in our database.");
                        userState.Remove(message.Chat.Id);
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
                        await botClient.SendTextMessageAsync(message.Chat, "Hello!!!👋\nI am your telegram bot for currency exchange. I was created to help you track currency exchange rates and learn up-to-date information about the currency market.\r\nI am always in touch and ready to answer your questions and offer the best currency exchange options. If you want to see all currencies /currensies.\r\nThank you for being with us!🤑");
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
                    else if (message.Text == "Exit")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, text: "Choose options:", replyMarkup: keyboard);
                    }
                    else if (message.Text == "/currensies")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, text: "ARS - Argentine Peso\r\nAMD - Armenian Dram\r\nAWG - Aruban Florin\r\nAUD - Australian Dollar\r\nAZN - Azerbaijan Manat\r\nBSD - Bahamian Dollar\r\nBHD - Bahraini Dinar\r\nPAB - Balboa\r\nBBD - Barbados Dollar\r\nBYN - Belarusian Ruble\r\nBZD - Belize Dollar\r\nBMD - Bermudian Dollar\r\nBOB - Boliviano\r\nBRL - Brazilian Real\r\nBND - Brunei Dollar\r\nBGN - Bulgarian Lev\r\nBIF - Burundi Franc\r\nXOF - CFA Franc BCEAO\r\nXAF - CFA Franc BEAC\r\nXPF - CFP Franc\r\nCVE - Cabo Verde Escudo\r\nCAD - Canadian Dollar\r\nKYD - Cayman Islands Dollar\r\nCLP - Chilean Peso\r\nCOP - Colombian Peso\r\nKMF - Comorian Franc\r\nCDF - Congolese Franc\r\nBAM - Convertible Mark\r\nNIO - Cordoba Oro\r\nCRC - Costa Rican Colon\r\nCUP - Cuban Peso\r\nCZK - Czech Koruna\r\nGMD - Dalasi\r\nDKK - Danish Krone\r\nMKD - Denar\r\nDJF - Djibouti Franc\r\nDOP - Dominican Peso\r\nVND - Dong\r\nXCD - East Caribbean Dollar\r\nEGP - Egyptian Pound\r\nSVC - El Salvador Colon\r\nETB - Ethiopian Birr\r\nEUR - Euro\r\nFKP - Falkland Islands Pound\r\nFJD - Fiji Dollar\r\nHUF - Forint\r\nGHS - Ghana Cedi\r\nGIP - Gibraltar Pound\r\nXAG - Gold\r\nHTG - Gourde\r\nPYG - Guarani\r\nGGP - Guernsey pound\r\nGNF - Guinean Franc\r\nGYD - Guyana Dollar\r\nHKD - Hong Kong Dollar\r\nUAH - Hryvnia\r\nISK - Iceland Krona\r\nINR - Indian Rupee\r\nIRR - Iranian Rial\r\nIQD - Iraqi Dinar\r\nJMD - Jamaican Dollar\r\nJEP - Jersey pound\r\nJOD - Jordanian Dinar\r\nKES - Kenyan Shilling\r\nPGK - Kina\r\nHRK - Kuna\r\nKWD - Kuwaiti Dinar\r\nAOA - Kwanza\r\nMMK - Kyat\r\nLAK - Lao Kip\r\nGEL - Lari\r\nLBP - Lebanese Pound\r\nALL - Lek\r\nHNL - Lempira\r\nSLL - Leone\r\nLRD - Liberian Dollar\r\nLYD - Libyan Dinar\r\nSZL - Lilangeni\r\nLSL - Loti\r\nMGA - Malagasy Ariary\r\nMWK - Malawi Kwacha\r\nMYR - Malaysian Ringgit\r\nMUR - Mauritius Rupee\r\nMXN - Mexican Peso\r\nMDL - Moldovan Leu\r\nMAD - Moroccan Dirham\r\nMZN - Mozambique Metical\r\nNGN - Naira\r\nERN - Nakfa\r\nNAD - Namibia Dollar\r\nNPR - Nepalese Rupee\r\nANG - Netherlands Antillean Guilder\r\nILS - New Israeli Sheqel\r\nTWD - New Taiwan Dollar\r\nNZD - New Zealand Dollar\r\nBTN - Ngultrum\r\nKPW - North Korean Won\r\nNOK - Norwegian Krone\r\nPKR - Pakistan Rupee\r\nMOP - Pataca\r\nTOP - Pa’anga\r\nCUC - Peso Convertible\r\nUYU - Peso Uruguayo\r\nPHP - Philippine Peso\r\nGBP - Pound Sterling\r\nBWP - Pula\r\nQAR - Qatari Rial\r\nGTQ - Quetzal\r\nZAR - Rand\r\nOMR - Rial Omani\r\nKHR - Riel\r\nRON - Romanian Leu\r\nMVR - Rufiyaa\r\nIDR - Rupiah\r\nRWF - Rwanda Franc\r\nSHP - Saint Helena Pound\r\nSAR - Saudi Riyal\r\nRSD - Serbian Dinar\r\nSCR - Seychelles Rupee\r\nSGD - Singapore Dollar\r\nPEN - Sol\r\nSBD - Solomon Islands Dollar\r\nKGS - Som\r\nSOS - Somali Shilling\r\nTJS - Somoni\r\nXDR - Special drawing rights\r\nLKR - Sri Lanka Rupee\r\nSDG - Sudanese Pound\r\nSRD - Surinam Dollar\r\nSEK - Swedish Krona\r\nCHF - Swiss Franc\r\nSYP - Syrian Pound\r\nBDT - Taka\r\nWST - Tala\r\nTZS - Tanzanian Shilling\r\nKZT - Tenge\r\nTTD - Trinidad and Tobago Dollar\r\nMNT - Tugrik\r\nTND - Tunisian Dinar\r\nTRY - Turkish Lira\r\nTMT - Turkmenistan New Manat\r\nAED - UAE Dirham\r\nUSD - US Dollar\r\nUGX - Uganda Shilling\r\nCLF - Unidad de Fomento\r\nUZS - Uzbekistan Sum\r\nVUV - Vatu\r\nKRW - Won\r\nYER - Yemeni Rial\r\nJPY - Yen\r\nCNY - Yuan Renminbi\r\nZMW - Zambian Kwacha\r\nZWL - Zimbabwe Dollar\r\nPLN - Zloty");
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

