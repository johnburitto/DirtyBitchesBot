using Telegram.Bot.Types;
using Telegram.Bot;
using System.Reflection;

namespace DirtyBitchesBot.Bot.Handlers
{
    public interface ITelegramBotHandlers
    {
        Task MessagesHandlerAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken);
        Task ErrorHandlerAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken);
        void AddCommands(Assembly assembly);
    }
}
