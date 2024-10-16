using DirtyBitchesBot.Commands.Base;
using DirtyBitchesBot.Utilities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DirtyBitchesBot.Commands
{
    public class StartCommand : MessageCommand
    {
        protected override List<string> Names { get; set; } = ["/start"];

        public override async Task ExetureAsync(ITelegramBotClient client, Message? message)
        {
            await client.SendTextMessageAsync(message!.Chat.Id, "Привіт 😊", replyMarkup: Keyboards.MainKeyboard);
        }
    }
}
