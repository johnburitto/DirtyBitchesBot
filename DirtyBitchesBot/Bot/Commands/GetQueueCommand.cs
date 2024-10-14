using DirtyBitchesBot.Bot.Commands.Base;
using DirtyBitchesBot.Extentions;
using DirtyBitchesBot.HttpInfrastructure;
using DirtyBitchesBot.HttpInfrastructure.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DirtyBitchesBot.Bot.Commands
{
    public class GetQueueCommand : MessageCommand
    {
        protected override List<string> Names { get; set; } = ["/queue"];

        public override async Task ExetureAsync(ITelegramBotClient client, Message? message)
        {
            var queue = await RequestClient.Instance.GetLaundryQueueAsync(DateTime.UtcNow);

            await client.SendTextMessageAsync(message!.Chat.Id, queue!.ToQueueList(message.From!.Id), parseMode: ParseMode.MarkdownV2);
        }
    }
}
