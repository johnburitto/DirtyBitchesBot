using Telegram.Bot;
using Telegram.Bot.Types;

namespace DirtyBitchesBot.Bot.Commands.Base
{
    public class MessageCommand : Command
    {
        public override async Task<bool> TryExecuteAsync(ITelegramBotClient client, Message? message)
        {
            if (CanBeExecuted(message?.Text ?? ""))
            {
                await ExetureAsync(client, message);

                return true;
            }

            return false;
        }
    }
}
