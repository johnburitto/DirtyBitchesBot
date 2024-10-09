using DirtyBitchesBot.Bot.Commands.Base;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DirtyBitchesBot.Bot.Handlers
{
    public class TelegramBotHandlers : ITelegramBotHandlers
    {
        private List<Command?> _commands;

        public TelegramBotHandlers() 
        { 
            _commands = [];
        }

        public async Task MessagesHandlerAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        await MessageHandlerAsync(client, update.Message);
                    }
                    return;
                default: return;
            }
        }

        public Task ErrorHandlerAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram Bot API exception:\n {apiRequestException.ErrorCode}\n {apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);

            return Task.CompletedTask;
        }

        public void AddCommands(Assembly assembly)
        {
            var commandsTypes = assembly.GetTypes()
                                        .Where(type => type.Namespace == "DirtyBitchesBot.Bot.Commands" && !type.IsNested)
                                        .ToList();

            commandsTypes.ForEach(type => _commands.Add((Command?)Activator.CreateInstance(type)));
        }

        private async Task MessageHandlerAsync(ITelegramBotClient client, Message? message)
        {
            foreach (var command in _commands)
            {
                if (await command!.TryExecuteAsync(client, message))
                {
                    return;
                }
            }
        }
    }
}
