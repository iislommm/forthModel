
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Api
{
    public class Program
    {
        private static string BotToken = "7653083369:AAE6Zz1A-Pk3YfBdjr5lHhhNLM9rpLhw2KE";
        private static TelegramBotClient BotClient = new TelegramBotClient(BotToken);
        private static string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "UserIds");
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
            if (update.Message == null) return;
            Ids = await GetAllIds();
            var message = update.Message;
            var user = message.Chat;
            Console.WriteLine(user.FirstName);
            if (message.Text == "/start")
            {
                await bot.SendTextMessageAsync(user.Id, "welcome", cancellationToken: cancellationToken);
            }
        }

        static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

        }
        public static async Task SaveUsers()
        {
            var users = JsonSerializer.Serialize(Ids);
            await File.WriteAllTextAsync(FilePath, users);
        }
        public static async Task<HashSet<long>> GetAllIds()
        {
            var idsString = File.ReadAllText(FilePath);
            var ids = JsonSerializer.Deserialize<HashSet<long>>(idsString);
            return ids ?? new HashSet<long>();
        }
    }
}
