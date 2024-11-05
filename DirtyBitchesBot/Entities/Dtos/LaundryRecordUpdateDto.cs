namespace DirtyBitchesBot.Entities.Dtos
{
    public class LaundryRecordUpdateDto
    {
        public Guid Uuid { get; set; }
        public long TelegramId { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime Date { get; set; }
        public string? Floor { get; set; }
    }
}
