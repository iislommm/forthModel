using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Polling;
using Telegram.Bot.Exceptions;

class Program
{
    private static string Token = "7653083369:AAE6Zz1A-Pk3YfBdjr5lHhhNLM9rpLhw2KE";
    private static TelegramBotClient bot = new TelegramBotClient(Token);
    private static Dictionary<long, (string choice, int score)> players = new();
    private static List<long> waitingPlayers = new();
    private static Dictionary<long, long> activeGames = new();
    private static Random random = new();

    static void Main(string[] args)
    {
        var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
        bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions);
        Console.WriteLine("Bot ishga tushdi...");
        Console.ReadLine();
    }

    static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message && update.Message.Text == "/start")
        {
            var keyboard = new InlineKeyboardMarkup(new[] { InlineKeyboardButton.WithCallbackData("O‘yinni boshlash", "start_game") });
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "O‘yinga xush kelibsiz!", replyMarkup: keyboard);
        }
        else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
        {
            var callbackData = update.CallbackQuery.Data;
            long userId = update.CallbackQuery.From.Id;

            if (callbackData == "start_game")
            {
                await StartGame(userId);
            }
            else if (callbackData == "rock" || callbackData == "paper" || callbackData == "scissors")
            {
                await HandlePlayerChoice(userId, callbackData);
            }
        }
    }

    static async Task StartGame(long userId)
    {
        if (waitingPlayers.Count > 0 && waitingPlayers[0] != userId)
        {
            long oppenentId = waitingPlayers[0];
            waitingPlayers.RemoveAt(0);
            activeGames[userId] = oppenentId;
            activeGames[oppenentId] = userId;
            players[userId] = ("", 0);
            players[oppenentId] = ("", 0);
            await SendGameMessage(userId);
            await SendGameMessage(oppenentId);

        }
        else
        {
            waitingPlayers.Add(userId);
            await bot.SendTextMessageAsync(userId, "Raqib qidirilmoqda...");
        }
    }

    static async Task SendGameMessage(long userId)
    {
        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("Tosh 🪨", "rock"), InlineKeyboardButton.WithCallbackData("Qaychi ✂️", "scissors"), InlineKeyboardButton.WithCallbackData("Qog‘oz 📃", "paper") }
        });
        await bot.SendTextMessageAsync(userId, "Tanlovingizni 5 soniya ichida qiling!", replyMarkup: keyboard);
        await Task.Delay(3000);
        if (players[userId].choice == "")
        {
            string[] options = { "rock", "paper", "scissors" };
            string randomChoice = options[random.Next(0, 5)];
            players[userId] = (randomChoice, players[userId].score);
        }
        await CheckRoundResult(userId);
    }

    static async Task HandlePlayerChoice(long userId, string choice)
    {
        if (players.ContainsKey(userId))
        {
            players[userId] = (choice, players[userId].score);
            await CheckRoundResult(userId);
        }
    }

    static async Task CheckRoundResult(long userId)
    {
        if (!activeGames.ContainsKey(userId)) return;
        long opponentId = activeGames[userId];
        if (!players.ContainsKey(opponentId) || players[opponentId].choice == "") return;

        string playerChoice = players[userId].choice;
        string opponentChoice = players[opponentId].choice;
        int playerScore = players[userId].score;
        int opponentScore = players[opponentId].score;

        string result = "";
        if (playerChoice == opponentChoice) result = "Durrang!";
        else if ((playerChoice == "rock" && opponentChoice == "scissors") ||
                 (playerChoice == "scissors" && opponentChoice == "paper") ||
                 (playerChoice == "paper" && opponentChoice == "rock"))
        {
            result = "Siz yutdingiz!";
            playerScore++;
        }
        else
        {
            result = "Raqib yutdi!";
            opponentScore++;
        }

        players[userId] = ("", playerScore);
        players[opponentId] = ("", opponentScore);
        await bot.SendTextMessageAsync(userId, $"{result} Siz: {playerScore} - {opponentScore} Raqib");
        await bot.SendTextMessageAsync(opponentId, $"{result} Siz: {opponentScore} - {playerScore} Raqib");

        if (playerScore == 3 || opponentScore == 3)
        {
            string finalMessage = playerScore > opponentScore ? "Siz g‘alaba qozondingiz!" : "Siz yutqazdingiz!";
            await bot.SendTextMessageAsync(userId, finalMessage);
            await bot.SendTextMessageAsync(opponentId, finalMessage);
            var restartKeyboard = new InlineKeyboardMarkup(new[] { InlineKeyboardButton.WithCallbackData("Yana o‘ynash", "start_game") });
            await bot.SendTextMessageAsync(userId, "O‘yin tugadi!", replyMarkup: restartKeyboard);
            await bot.SendTextMessageAsync(opponentId, "O‘yin tugadi!", replyMarkup: restartKeyboard);
            activeGames.Remove(userId);
            activeGames.Remove(opponentId);
            players.Remove(userId);
            players.Remove(opponentId);
        }
        else
        {
            await SendGameMessage(userId);
            await SendGameMessage(opponentId);
        }
    }

    static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception.ToString());
        return Task.CompletedTask;
    }
}
