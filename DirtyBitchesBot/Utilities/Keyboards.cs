using Telegram.Bot.Types.ReplyMarkups;

namespace DirtyBitchesBot.Utilities
{
    public static class Keyboards
    {
        public static ReplyKeyboardMarkup MainKeyboard => new([
            new KeyboardButton[] { "Вільні години", "Черга", "Записатися" },
            new KeyboardButton[] { "Звільнити черну", "Зсунути черну" }
        ])
        {
            ResizeKeyboard = true
        };
        public static ReplyKeyboardMarkup FloorsKeyboard => new([
            new KeyboardButton[] { "1", "2", "3", "4", "5" }
        ])
        {
            ResizeKeyboard = true
        };
        public static ReplyKeyboardMarkup DatesKeyboard => GetDatesKeyboard();

        private static ReplyKeyboardMarkup GetDatesKeyboard()
        {
            var currentDate = DateTime.Now;

            return new(Enumerable.Range(currentDate.Day, DateTime.DaysInMonth(currentDate.Year, currentDate.Month) - currentDate.Day)
                             .Select(day => new DateTime(currentDate.Year, currentDate.Month, day))
                             .Select((date, index) => new { Date = date.ToString("yyyy-MM-dd"), Index = index })
                             .GroupBy(obj => obj.Index / 5)
                             .Select(segment => segment.Select(obj => new KeyboardButton(obj.Date)))
                             .ToList())
            {
                ResizeKeyboard = true
            };
        }
    }
}
