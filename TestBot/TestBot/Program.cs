
using Telegram.Bot;

namespace TestBot
{
    public class Program
    {
        private static string BotToken = "7653083369:AAE6Zz1A-Pk3YfBdjr5lHhhNLM9rpLhw2KE";
        private static TelegramBotClient BotClient = new TelegramBotClient(BotToken);
        private static string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "UserInformations.Json");
        public async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) 
                .Build();
            var serviceCollection = new ServiceCollection();

            var connectionString = configuration.GetSection("DatabaseConnection");
            object addDbContext = serviceCollection.AddDbContext<MainContext>(
                o => o.UseSqlServer((System.Data.Common.DbConnection)connectionString));
        }
    }
}
