using DirtyBitchesBot.Bot.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace DirtyBitchesBot.Bot
{
    public class TelegramBot
    {
        public TelegramBotClient? Bot { get; set; }
        public ReceiverOptions? ReceiverOptions { get; set; }
        public ITelegramBotHandlers? Handlers { get; set; }
        public string Token { get; set; } = string.Empty;

        public void StartReceiving()
        {
            Bot?.StartReceiving(Handlers!.MessagesHandlerAsync, Handlers!.ErrorHandlerAsync, ReceiverOptions);

            Console.ReadKey();
        }
    }
}
