namespace DirtyBitchesBot.Entities.Dtos
{
    public class LaundryRecordAddResponse
    {
        public Guid Id { get; set; }
        public long TelegramId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime Date { get; set; }
        public string? Room { get; set; }
        public int Floor { get; set; }
    }
}
