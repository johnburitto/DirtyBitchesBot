using System.Reflection;
using DirtyBitchesBot.Bot.Handlers;
using DirtyBitchesBot.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace DirtyBitchesBot.Bot
{
    public class BotBuilder
    {
        private TelegramBot _bot;
        private ConfigurationService? _configurationService;

        public BotBuilder()
        {
            _bot = new TelegramBot();
        }

        public BotBuilder AddConfiguration()
        {
            _configurationService = new ConfigurationService();

            return this;
        }

        public BotBuilder AddHandlers<THandlers>() where THandlers : ITelegramBotHandlers, new()
        {
            _bot.Handlers = new THandlers();

            return this;
        }

        public BotBuilder ConfigureOptions(Action<ReceiverOptions> optionsAction)
        {
            _bot.ReceiverOptions = new ReceiverOptions();
            optionsAction.Invoke(_bot.ReceiverOptions);

            return this;
        }

        public BotBuilder AddCommands(Assembly assembly)
        {
            _bot.Handlers?.AddCommands(assembly);

            return this;
        }

        public BotBuilder ConfigureBot()
        {
            _bot.Token = _configurationService?.GetValue<string>("Token") ?? "";
            _bot.Bot = new TelegramBotClient(_bot.Token);

            return this;
        }

        public TelegramBot Build()
        {
            return _bot;
        }
    }
}
