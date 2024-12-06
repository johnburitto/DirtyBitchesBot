using DirtyBitchesBot.Commands.Base;
using DirtyBitchesBot.Extentions;
using DirtyBitchesBot.HttpInfrastructure;
using DirtyBitchesBot.HttpInfrastructure.Extensions;
using DirtyBitchesBot.StateMachineBase;
using DirtyBitchesBot.Utilities;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DirtyBitchesBot.Commands
{
    public class GetQueueCommand : MessageCommand
    {
        protected override List<string> Names { get; set; } = ["/queue", "Черга", "queue_get_floor", "queue_get_date"];

        public override async Task ExetureAsync(ITelegramBotClient client, Message? message)
        {
            var state = await StateMachine.GetStateAsync($"{message!.From!.Id}_state");

            if (state == null)
            {
                state = new State
                {
                    StateName = "queue_get_floor"
                };
                await client.SendTextMessageAsync(message.From.Id, "Оберіть поверх:", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.FloorsKeyboard);
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "queue_get_floor")
            {
                state.StateName = "queue_get_date";
                state.StateObject!.Floor = message.Text;
                await client.SendTextMessageAsync(message.From.Id, "Оберіть дату:", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.DatesKeyboard);
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "queue_get_date")
            {
                var queue = await RequestClient.Instance.GetLaundryQueueAsync(DateTime.Parse(message.Text ?? ""), state.StateObject?.Floor as string);

                await client.SendTextMessageAsync(message.From.Id, queue!.ToQueueList(message.From.Id), parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.MainKeyboard);
                await StateMachine.RemoveStateAsync($"{message.From.Id}_state");
            }
        }
    }
}
