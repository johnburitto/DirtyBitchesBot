using DirtyBitchesBot.Entities;
using Newtonsoft.Json;

namespace DirtyBitchesBot.HttpInfrastructure.Extensions
{
    public static class LaundryGetExtensions
    {
        public static async Task<Dictionary<string, LaundryRecord>?> GetLaundryQueueAsync(this RequestClient client, DateTime date)
        {
            var response = await client.Client.GetAsync($"/laundry-queue?date={date:yyyy-MM-dd}");

            return JsonConvert.DeserializeObject<Dictionary<string, LaundryRecord>>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<List<string>?> GetFreeHoursAsync(this RequestClient client, DateTime date)
        {
            var response = await client.Client.GetAsync($"laundry-queue/available?date={date:yyyy-MM-dd}");

            return JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
        }
    }
}
