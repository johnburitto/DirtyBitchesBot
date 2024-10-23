using DirtyBitchesBot.Entities;
using Telegram.Bot.Types.ReplyMarkups;

namespace DirtyBitchesBot.Extentions
{
    public static class ListExtensions
    {
        public static string ToFreeHoursList(this List<string> hours)
        {
            return $"*Вільні години *:\n\n{string.Join("\n", hours.Select(hour => $"✅ {hour}"))}";
        }

        public static ReplyKeyboardMarkup ToFreeHourKeyboard(this List<string> hours)
        {
            return new(hours.Select((hour, index) => new { Hour = hour, Index = index })
                             .GroupBy(obj => obj.Index / 5)
                             .Select(segment => segment.Select(obj => new KeyboardButton(obj.Hour)))
                             .ToList());
        }

        public static string ToUserRecordsList(this List<UserRecord> records)
        {
            return $"*Мої записи:*\n\n{string.Join("\n", records.Select((record, index) => $"Запис \\#{index + 1} 🧼🫧 {record}"))}";
        }
    }
}
