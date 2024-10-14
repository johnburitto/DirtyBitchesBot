using DirtyBitchesBot.Entities;

namespace DirtyBitchesBot.Extentions
{
    public static class DictionaryExtensions
    {
        public static string ToQueueList(this Dictionary<string, LaundryRecord> queue, long userTelegramId)
        {
            return $"*Черга:*\n\n{string.Join("\n", queue.Select(record => $"{GetEmoji(record.Value, userTelegramId)} {record.Key} \\| " + 
                        (record.Value == null ? "Вільно" : record.Value.ToString())))}";
        }

        private static string GetEmoji(LaundryRecord? record, long userTelegramId)
        {
            return record != null ? userTelegramId == record.TelegramId ? "👤" : "❌" : "✅";
        }
    }
}
