using Telegram.Bot;
using Telegram.Bot.Polling;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace ConvertCurrencyBot;

public class ProgramBot
{
    static void Main(string[] args)
    {
        string settingsPath = Path.Combine(Environment.CurrentDirectory, "appsettings.json");
        var config = new ConfigurationBuilder().AddJsonFile(settingsPath).Build();

        TelegramBot bot = new(config["DefaultAPIUrl"]!, config["TelegramBotToken"]!);
        Console.WriteLine("Запущен бот " + bot.Bot.GetMeAsync().Result.FirstName);

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };
        bot.Bot.StartReceiving(
            bot.HandleUpdateAsync,
            HandleError,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
    }

    public static Task HandleError(ITelegramBotClient bot, Exception exception, CancellationToken token)
    {
        Console.WriteLine("Error accured: " + exception.Message);
        return Task.CompletedTask;
    }

}



