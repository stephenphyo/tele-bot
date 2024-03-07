using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegrambot;
using static Telegrambot.Model.RedditModel;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        
        var bot = new TelegramBotClient("6303059270:AAG39WQJfEfKtwVoaIWkv-vywsXyfop6CwY");
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new UpdateType[]
            {
                UpdateType.Message,
                UpdateType.EditedMessage,
            }
        };
        bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions);

        Console.ReadLine();
    }

    static async Task ErrorHandler(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"An error occurred: {exception.Message}");
    }

    static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message)
        {
            if (update.Message.Type == MessageType.Text)
            {
                var text = update.Message.Text;
                var chatId = update.Message.Chat.Id;
                var username = update.Message.Chat.FirstName;
                Console.WriteLine($"{username}|{chatId}|{text}");
                string replyText = string.Empty;
                if (text.ToLower() == "/start")
                {
                    Console.WriteLine($"{username}|{chatId}|{text}");
                    replyText = $"Hello Hello{username}, Welcome to Trending Reddit Bot.\n";
                    replyText += "Hottest Reddit Meme Post Right now              - /Trend.\n";
                    replyText += "Hottest Reddit Meme within 1month               -/HottestMonth.\n";
                    replyText += "Get Top 20 Reddit Posts wihtin 24 hrs with Pdf  - /PDF.";
                    
                }
                else if (text.ToLower() == "/trend")
                {
                    TrendingReddit telegrambot = new TrendingReddit();
                    TrendViewModel topMemeTitle = await telegrambot.GetMemes();
                    Console.WriteLine($"Top meme title: {topMemeTitle.Title}");
                    replyText = $"🔥 TOP TRENDING REDDIT RIGHT NOW. 🔥\n"; 
                    replyText += $"Titel {topMemeTitle.Title} \n " +
                                  $"Url {topMemeTitle.Url}\n";

                }
                else if (text.ToLower() == "/hottestmonth")
                {
                    TrendingReddit telegrambot = new TrendingReddit();
                    TrendViewModel topMemeTitle = await telegrambot.GetMemesWithinOneMonth();
                    Console.WriteLine($"Top meme title: {topMemeTitle.Title}");
                    replyText = $"🔥 TOP TRENDING REDDIT ONE MONTH. 🔥\n";
                    replyText += $"Titel {topMemeTitle.Title} \n " +
                                  $"Url {topMemeTitle.Url}\n";

                }
                else if (text.ToLower() == "/hottestyear")
                {
                    TrendingReddit telegrambot = new TrendingReddit();
                    TrendViewModel topMemeTitle = await telegrambot.GetMemesWithinOneMonth();
                    Console.WriteLine($"Top meme title: {topMemeTitle.Title}");
                    replyText = $"🔥 TOP TRENDING REDDIT ONE YEAR. 🔥\n";
                    replyText += $"Titel {topMemeTitle.Title} \n " +
                                  $"Url {topMemeTitle.Url}\n";

                }
                else if(text.ToLower() =="/pdf"){
                    TrendTopTwentyReddit topTwenty = new TrendTopTwentyReddit();
                    TrendViewModel topMemeTitle = await topTwenty.GetMemes(chatId);
                    replyText = $"🔥 TOP TRENDING 20 REDDIT MEMES RIGHT NOW. 🔥\n";

                }
                else
                {
                    replyText = $"Wrong Command.\n";
                    replyText += "Choose command \n /Start " +
                        "\n /Trend \n/HottestMonth \n /TopPdf \n";
                }
                

                await bot.SendTextMessageAsync(chatId, replyText, cancellationToken: cancellationToken);
            }
        }
    }
}
