using DirtyBitchesBot.Bot.Commands.Base;
using DirtyBitchesBot.Extentions;
using DirtyBitchesBot.HttpInfrastructure;
using DirtyBitchesBot.HttpInfrastructure.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DirtyBitchesBot.Bot.Commands
{
    public class GetFreeHoursCommand : MessageCommand
    {
        protected override List<string> Names { get; set; } = ["/free-hours"];

        public override async Task ExetureAsync(ITelegramBotClient client, Message? message)
        {
            var hours = await RequestClient.Instance.GetFreeHoursAsync(DateTime.UtcNow);

            await client.SendTextMessageAsync(message!.Chat.Id, hours!.ToFreeHoursList(), parseMode: ParseMode.MarkdownV2);
        }
    }
}
