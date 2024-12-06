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
    public class GetUserRecordsCommand : MessageCommand
    {
        protected override List<string> Names { get; set; } = ["Мої записи", "ur_get_floor"];

        public override async Task ExetureAsync(ITelegramBotClient client, Message? message)
        {
            var state = await StateMachine.GetStateAsync($"{message!.From!.Id}_state");

            if (state == null)
            {
                state = new State
                {
                    StateName = "ur_get_floor"
                };
                await client.SendTextMessageAsync(message.From.Id, "Оберіть поверх:", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.FloorsKeyboard);
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "ur_get_floor")
            {
                var records = await RequestClient.Instance.GetUserRecordsAsync(message.From.Id, message.Text);

                await client.SendTextMessageAsync(message.From.Id, records!.ToUserRecordsList(), parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.MainKeyboard);
                await StateMachine.RemoveStateAsync($"{message.From.Id}_state");
            }
        }
    }
}
