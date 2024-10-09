using DirtyBitchesBot.Bot;
using DirtyBitchesBot.Bot.Handlers;
using System.Reflection;

var botBuilder = new BotBuilder();

botBuilder.AddConfiguration()
          .AddHandlers<TelegramBotHandlers>()
          .AddCommands(Assembly.GetExecutingAssembly())
          .ConfigureOptions(options =>
          {
              options.AllowedUpdates = [];
              options.ThrowPendingUpdates = true;
          })
          .ConfigureBot();

var bot = botBuilder.Build();

bot.StartReceiving();