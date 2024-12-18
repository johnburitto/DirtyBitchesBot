﻿using DirtyBitchesBot.StateMachineBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DirtyBitchesBot.Commands.Base
{
    public class MessageCommand : Command
    {
        public override async Task<bool> TryExecuteAsync(ITelegramBotClient client, Message? message)
        {
            try
            {
                var state = await StateMachine.GetStateAsync($"{message!.From!.Id}_state");
                var data = state == null ? message.Text : state.StateName;

                if (CanBeExecuted(data ?? ""))
                {
                    await ExetureAsync(client, message);

                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                await client.SendTextMessageAsync(message!.From!.Id, e.Message);

                return false;
            }
        }
    }
}
