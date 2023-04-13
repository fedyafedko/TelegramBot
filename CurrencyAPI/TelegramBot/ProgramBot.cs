using Telegram.Bot;
using Telegram.Bot.Polling;
using Newtonsoft.Json;


namespace ConvertCurrencyBot
{
    public class ProgramBot
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + TelegramBot.bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            TelegramBot.bot.StartReceiving(
                TelegramBot.HandleUpdateAsync,
                TelegramBot.HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

            Console.ReadLine();
        }

    }
}



