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
    public class GetFreeHoursCommand : MessageCommand
    {
        protected override List<string> Names { get; set; } = ["/free-hours", "Вільні години", "fh_get_floor", "fh_get_date"];

        public override async Task ExetureAsync(ITelegramBotClient client, Message? message)
        {
            var state = await StateMachine.GetStateAsync($"{message!.From!.Id}_state");

            if (state == null) 
            {
                state = new State
                {
                    StateName = "fh_get_floor"
                };
                await client.SendTextMessageAsync(message.From.Id, "Оберіть поверх:", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.FloorsKeyboard);
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "fh_get_floor")
            {
                state.StateName = "fh_get_date";
                state.StateObject!.Floor = message.Text;
                await client.SendTextMessageAsync(message.From.Id, "Оберіть дату:", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.DatesKeyboard);
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "fh_get_date")
            {
                var hours = await RequestClient.Instance.GetFreeHoursAsync(DateTime.Parse(message.Text ?? ""), state.StateObject?.Floor as string);

                await client.SendTextMessageAsync(message.From.Id, hours!.ToFreeHoursList(), parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.MainKeyboard);
                await StateMachine.RemoveStateAsync($"{message.From.Id}_state");
            }
        }
    }
}
