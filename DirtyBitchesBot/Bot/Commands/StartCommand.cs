using DirtyBitchesBot.Bot.Commands.Base;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DirtyBitchesBot.Bot.Commands
{
    public class StartCommand : MessageCommand
    {
        protected override List<string> Names { get; set; } = ["/start"];

        public override async Task ExetureAsync(ITelegramBotClient client, Message? message)
        {
            await client.SendTextMessageAsync(message!.Chat.Id, "Hello World!!!");
        }
    }
}
