namespace DirtyBitchesBot.Entities.Dtos
{
    public class LaundryRecordCreateDto
    {
        public long TelegramId { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime Date { get; set; }
        public string? Room { get; set; }
        public string? Floor { get; set; }
    }
}
