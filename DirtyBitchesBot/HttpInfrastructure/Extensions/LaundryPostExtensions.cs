using DirtyBitchesBot.Entities.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace DirtyBitchesBot.HttpInfrastructure.Extensions
{
    public static class LaundryPostExtensions
    {
        public static async Task<LaundryRecordAddResponse?> AddRecordAsync(this RequestClient client, LaundryRecordCreateDto? dto, string? floor = "")
        {
            client.Client.DefaultRequestHeaders.Remove("floor");
            client.Client.DefaultRequestHeaders.Add("floor", floor);

            var dtoString = JsonConvert.SerializeObject(dto, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var content = new StringContent(dtoString, Encoding.UTF8, "application/json");
            var response = await client.Client.PostAsync("/laundry-queue", content);

            Console.WriteLine(dtoString);

            return JsonConvert.DeserializeObject<LaundryRecordAddResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}
