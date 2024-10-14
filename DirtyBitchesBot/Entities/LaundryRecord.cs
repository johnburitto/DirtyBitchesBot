namespace DirtyBitchesBot.Entities
{
    public class LaundryRecord
    {
        public Guid Id { get; set; }
        public long TelegramId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Room { get; set; }
        public int Floor { get; set; }

        public override string ToString()
        {
            return $"{FullName}\\[@{UserName}\\] з {Room}, {Floor}";
        }
    }
}
