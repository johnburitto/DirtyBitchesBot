using Telegram.Bot;
using Telegram.Bot.Types;

namespace DirtyBitchesBot.Bot.Commands.Base
{
    public abstract class Command
    {
        protected virtual List<string> Names { get; set; } = [];

        public virtual Task ExetureAsync(ITelegramBotClient client, Message? message)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> TryExecuteAsync(ITelegramBotClient client, Message? message)
        {
            throw new NotImplementedException();
        }

        public virtual bool CanBeExecuted(string message)
        {
            return Names.Any(name => message.ToLower().Contains(name.ToLower()));
        } 
    }
}
