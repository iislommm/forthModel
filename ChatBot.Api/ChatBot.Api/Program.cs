using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChatBot.Api
{
    internal class Program
    {
        private static string BotToken = "7653083369:AAE6Zz1A-Pk3YfBdjr5lHhhNLM9rpLhw2KE";
        private static TelegramBotClient BotClient = new TelegramBotClient(BotToken);
        private static string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "userIds");
        //private static List<long> Ids = new List<long>();
        private static HashSet<long> Ids = new HashSet<long>();
        static async Task Main(string[] args)
        {
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "[]");
            }
            var recieverOptions = new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message, UpdateType.InlineQuery } };
            Console.WriteLine("Done");
            BotClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                recieverOptions);
            Console.ReadKey();
        }
        static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                Ids = await GetAllIds();
                var message = update.Message;
                var user = message.Chat;
                Console.WriteLine(user.Id);
                if (message.Text == "/start")
                {
                    await SaveUserId();
                    Ids.Add(user.Id);



                    var menu = new ReplyKeyboardMarkup(new[]
                    {
                   new KeyboardButton[] { "Option 1", "Option 2" },
                   new KeyboardButton[] { "Option 3", "Option 4" }
                })

                    {
                        ResizeKeyboard = true,
                        OneTimeKeyboard = true
                    };

                    //var menu = new ReplyKeyboardMarkup(new[]
                    //{
                    //    new []
                    //    {
                    //        new KeyboardButton ("Option 1"),
                    //        new KeyboardButton("Option 2")
                    //    },
                    //    new[]
                    //    {
                    //        new KeyboardButton("Option 3"),
                    //        new KeyboardButton("Option 4")
                    //    }

                    //});    

                    await bot.SendTextMessageAsync(user.Id, "Assalomu alaikum, welcome", replyMarkup: menu);
                    return;
                }
                if (message.Text == "Option 1")
                {
                    var inlineMenu = new InlineKeyboardMarkup(new[]
                    {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Option 1", "option_1"),
                        InlineKeyboardButton.WithCallbackData("Option 2", "option_2")
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Option 3", "option_3"),
                        InlineKeyboardButton.WithCallbackData("Option 4", "option_4")
                    }
                });
                    await bot.SendTextMessageAsync(user.Id, "Assalomu alaikum, welcome", replyMarkup: inlineMenu);
                }

            }
            //else if (update.Type == UpdateType.CallbackQuery)
            //{
            //    var message = update.CallbackQuery.Message;
            //    var id = update.CallbackQuery.From.Id;
            //    await bot.SendTextMessageAsync(id, $"Your option is {message}");
            //}
            else if (update.Type == UpdateType.CallbackQuery)
            {
                var message = update.CallbackQuery.Message;
                var id = update.CallbackQuery.From.Id;
                await bot.SendTextMessageAsync(id, $"Your option is {update.CallbackQuery.Message}");
            }
        }

        static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

        }
        public static async Task SaveUserId()
        {
            var serializedIds = JsonSerializer.Serialize(Ids);
            await File.WriteAllTextAsync(FilePath, serializedIds);
        }
        public static async Task<HashSet<long>> GetAllIds()
        {
            var idsString = File.ReadAllText(FilePath);
            var ids = JsonSerializer.Deserialize<HashSet<long>>(idsString);
            return ids ?? new HashSet<long>();
        }

    }
}
