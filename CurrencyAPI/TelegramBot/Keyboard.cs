using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;

namespace TelegramBot
{
    public class Keyboard
    {
        public static IReplyMarkup GetButtons()
        {
            KeyboardButton buttonAllCurrency = new KeyboardButton("All My Currency");
            KeyboardButton buttonUSD = new KeyboardButton("USD");
            KeyboardButton buttonEUR = new KeyboardButton("EUR");
            KeyboardButton buttonGBR = new KeyboardButton("GBP");
            KeyboardButton buttonPLN = new KeyboardButton("PLN");
            KeyboardButton buttonCalculator = new KeyboardButton("Сalculator");


            KeyboardButton[][] buttons = new KeyboardButton[][]
            {
                new KeyboardButton[] { buttonAllCurrency},
                new KeyboardButton[] { buttonUSD, buttonEUR, buttonGBR, buttonPLN },
                new KeyboardButton[] { buttonCalculator}
            };

            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(buttons);

            return keyboard;
        }
        public static IReplyMarkup GetButtonsUpdate()
        {
            KeyboardButton buttonUpdate = new KeyboardButton("Update My Currency");
            KeyboardButton buttonDelete = new KeyboardButton("Delete My Currency");
            KeyboardButton buttonExit = new KeyboardButton("Exit");



            KeyboardButton[][] buttons = new KeyboardButton[][]
            {
                new KeyboardButton[] { buttonDelete, buttonUpdate },
                new KeyboardButton[] { buttonExit}
            };

            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(buttons);

            return keyboard;
        }
    }
}
