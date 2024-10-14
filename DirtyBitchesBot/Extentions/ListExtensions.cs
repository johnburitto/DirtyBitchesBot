namespace DirtyBitchesBot.Extentions
{
    public static class ListExtensions
    {
        public static string ToFreeHoursList(this List<string> hours)
        {
            return $"*Вільні години *:\n\n{string.Join("\n", hours.Select(hour => $"✅ {hour}"))}";
        }
    }
}
