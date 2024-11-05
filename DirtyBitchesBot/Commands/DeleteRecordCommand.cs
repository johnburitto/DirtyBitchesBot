using DirtyBitchesBot.Commands.Base;
using DirtyBitchesBot.Entities.Dtos;
using DirtyBitchesBot.Extentions;
using DirtyBitchesBot.HttpInfrastructure;
using DirtyBitchesBot.HttpInfrastructure.Extensions;
using DirtyBitchesBot.StateMachineBase;
using DirtyBitchesBot.Utilities;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DirtyBitchesBot.Commands
{
    public class DeleteRecordCommand : MessageCommand
    {
        protected override List<string> Names { get; set; } = ["Звільнити чергу", "del_get_floor", "del_get_record"];

        public override async Task ExetureAsync(ITelegramBotClient client, Message? message)
        {
            var state = await StateMachine.GetStateAsync($"{message!.From!.Id}_state");

            if (state == null)
            {
                state = new State
                {
                    StateName = "del_get_floor", 
                    StateObject = new LaundryRecordDeleteDto
                    {
                        TelegramId = message.From.Id
                    }
                };
                await client.SendTextMessageAsync(message.From.Id, "Оберіть поверх:", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.FloorsKeyboard);
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "del_get_floor")
            {
                state.StateName = "del_get_record";
                state.StateObject.Floor = message.Text;

                var userRecords = await RequestClient.Instance.GetUserRecordsAsync((state.StateObject as LaundryRecordDeleteDto)!.TelegramId, state.StateObject.Floor as string);

                await client.SendTextMessageAsync(message.From.Id, userRecords!.ToUserRecordsList(), parseMode: ParseMode.MarkdownV2, replyMarkup: userRecords!.ToUserRecordsKeyboard());
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "del_get_record")
            {
                var recordId = int.Parse(Regex.Match(message.Text ?? "", @"[0-9]+").Value) - 1;
                var userRecords = await RequestClient.Instance.GetUserRecordsAsync((state.StateObject as LaundryRecordDeleteDto)!.TelegramId, state.StateObject.Floor as string);
                var record = userRecords?[recordId];

                state.StateObject.Uuid = record!.Uuid;
                await RequestClient.Instance.DeleteRecordAsync(state.StateObject as LaundryRecordDeleteDto);
                await client.SendTextMessageAsync(message.From.Id, "✅ Запис успішно видалено", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.MainKeyboard);
                await StateMachine.RemoveStateAsync($"{message.From.Id}_state");
            }
        }
    }
}
