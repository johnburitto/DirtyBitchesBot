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
    public class UpdateRecordCommand : MessageCommand
    {
        protected override List<string> Names { get; set; } = ["Зсунути чергу", "upd_get_floor", "upd_get_record", "upd_get_date", "upd_get_hour"];

        public override async Task ExetureAsync(ITelegramBotClient client, Message? message)
        {
            var state = await StateMachine.GetStateAsync($"{message!.From!.Id}_state");

            if (state == null)
            {
                state = new State
                {
                    StateName = "upd_get_floor",
                    StateObject = new LaundryRecordUpdateDto
                    {
                        TelegramId = message.From.Id
                    }
                };
                await client.SendTextMessageAsync(message.From.Id, "Оберіть поверх:", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.FloorsKeyboard);
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "upd_get_floor")
            {
                state.StateName = "upd_get_record";
                state.StateObject.Floor = message.Text;

                var userRecords = await RequestClient.Instance.GetUserRecordsAsync((state.StateObject as LaundryRecordUpdateDto)!.TelegramId, state.StateObject.Floor as string);

                await client.SendTextMessageAsync(message.From.Id, userRecords!.ToUserRecordsList(), parseMode: ParseMode.MarkdownV2, replyMarkup: userRecords!.ToUserRecordsKeyboard());
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "upd_get_record")
            {
                var recordId = int.Parse(Regex.Match(message.Text ?? "", @"[0-9]+").Value) - 1;
                var userRecords = await RequestClient.Instance.GetUserRecordsAsync((state.StateObject as LaundryRecordUpdateDto)!.TelegramId, state.StateObject.Floor as string);
                var record = userRecords?[recordId];

                state.StateName = "upd_get_date";
                state.StateObject.Uuid = record!.Uuid;
                await client.SendTextMessageAsync(message.From.Id, "Оберіть дату:", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.DatesKeyboard);
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "upd_get_date")
            {
                state.StateName = "upd_get_hour";
                state.StateObject.Date = DateTime.Parse(message.Text ?? "");

                var freeHours = await RequestClient.Instance.GetFreeHoursAsync((DateTime)state.StateObject.Date, state.StateObject.Floor as string);

                await client.SendTextMessageAsync(message.From.Id, "Оберіть годину:", parseMode: ParseMode.MarkdownV2, replyMarkup: freeHours?.ToFreeHourKeyboard());
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "upd_get_hour")
            {
                state.StateObject.Time = TimeSpan.Parse(message.Text ?? "");

                var response = await RequestClient.Instance.UpdateRecordAsync(state.StateObject as LaundryRecordUpdateDto);

                if (response == null)
                {
                    await client.SendTextMessageAsync(message.From.Id, "⚙️ Підчас перенесення виникла якась помилка", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.MainKeyboard);
                }
                else
                {
                    await client.SendTextMessageAsync(message.From.Id, "✅ Запис успішно перенесено", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.MainKeyboard);
                }

                await StateMachine.RemoveStateAsync($"{message.From.Id}_state");
            }
        }
    }
}
