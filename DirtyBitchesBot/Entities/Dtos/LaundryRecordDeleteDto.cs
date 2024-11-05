namespace DirtyBitchesBot.Entities.Dtos
{
    public class LaundryRecordDeleteDto
    {
        public long TelegramId { get; set; }
        public Guid Uuid { get; set; }
        public string? Floor { get; set; }
    }
}
