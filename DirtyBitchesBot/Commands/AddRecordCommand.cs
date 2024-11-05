using DirtyBitchesBot.Commands.Base;
using DirtyBitchesBot.Entities.Dtos;
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
    public class AddRecordCommand : MessageCommand
    {
        protected override List<string> Names { get; set; } = ["Записатися", "add_get_floor", "add_get_date", "add_get_hour", "add_get_room"];

        public override async Task ExetureAsync(ITelegramBotClient client, Message? message)
        {
            var state = await StateMachine.GetStateAsync($"{message!.From!.Id}_state");

            if (state == null)
            {
                state = new State
                {
                    StateName = "add_get_floor",
                    StateObject = new LaundryRecordCreateDto
                    {
                        TelegramId = message.From.Id,
                        FullName = $"{message.From.FirstName} {message.From.LastName}",
                        Username = message.From.Username
                    }
                };
                await client.SendTextMessageAsync(message.From.Id, "Оберіть поверх:", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.FloorsKeyboard);
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "add_get_floor")
            {
                state.StateName = "add_get_date";
                state.StateObject.Floor = message.Text;
                await client.SendTextMessageAsync(message.From.Id, "Оберіть дату:", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.DatesKeyboard);
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "add_get_date")
            {
                state.StateName = "add_get_hour";
                state.StateObject.Date = DateTime.Parse(message.Text ?? "");

                var freeHours = await RequestClient.Instance.GetFreeHoursAsync((DateTime)state.StateObject.Date, state.StateObject.Floor as string);

                await client.SendTextMessageAsync(message.From.Id, "Оберіть годину:", parseMode: ParseMode.MarkdownV2, replyMarkup: freeHours?.ToFreeHourKeyboard());
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "add_get_hour")
            {
                state.StateName = "add_get_room";
                state.StateObject.Time = TimeSpan.Parse(message.Text ?? "");
                await client.SendTextMessageAsync(message.From.Id, "Введіть номер кімнати:", parseMode: ParseMode.MarkdownV2, replyMarkup: null);
                await StateMachine.SetStateAsync($"{message.From.Id}_state", state);
            }
            else if (state.StateName == "add_get_room")
            {
                state.StateObject.Room = message.Text;

                var response = await RequestClient.Instance.AddRecordAsync(state.StateObject as LaundryRecordCreateDto, state.StateObject.Floor as string);

                if (response == null)
                {
                    await client.SendTextMessageAsync(message.From.Id, "⚙️ Підчас створення виникла якась помилка", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.MainKeyboard);
                }
                else
                {
                    await client.SendTextMessageAsync(message.From.Id, "✅ Запис успішно додано", parseMode: ParseMode.MarkdownV2, replyMarkup: Keyboards.MainKeyboard);
                }

                await StateMachine.RemoveStateAsync($"{message.From.Id}_state");
            }
        }
    }
}
