namespace DirtyBitchesBot.Entities
{
    public class UserRecord
    {
        public TimeSpan Time { get; set; }
        public DateTime Date { get; set; }
        public string? Room { get; set; }
        public int Floor { get; set; }

        public override string ToString()
        {
            return $"{Date.ToString("dd.MM.yyyy").Replace(".", "\\.")} на {Time.ToString(@"hh\:mm")}\\. {Room?.Replace("(", "\\(").Replace(")", "\\)")}, {Floor} поверх";
        }
    }
}
