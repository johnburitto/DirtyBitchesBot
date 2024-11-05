using DirtyBitchesBot.Entities.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
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

            return response.StatusCode == HttpStatusCode.BadRequest ? throw new Exception(await response.Content.ReadAsStringAsync()) 
                : JsonConvert.DeserializeObject<LaundryRecordAddResponse>(await response.Content.ReadAsStringAsync());
        }
        
        public static async Task<LaundryRecordAddResponse?> DeleteRecordAsync(this RequestClient client, LaundryRecordDeleteDto? dto)
        {
            var dtoString = JsonConvert.SerializeObject(dto, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var content = new StringContent(dtoString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, "/laundry-queue")
            {
                Content = content
            };
            var response = await client.Client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.Unauthorized ? throw new Exception(await response.Content.ReadAsStringAsync()) 
                : JsonConvert.DeserializeObject<LaundryRecordAddResponse>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<LaundryRecordAddResponse?> UpdateRecordAsync(this RequestClient client, LaundryRecordUpdateDto? dto, string? floor = "")
        {
            client.Client.DefaultRequestHeaders.Remove("floor");
            client.Client.DefaultRequestHeaders.Add("floor", floor);

            var dtoString = JsonConvert.SerializeObject(dto, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var content = new StringContent(dtoString, Encoding.UTF8, "application/json");
            var response = await client.Client.PutAsync("/laundry-queue", content);

            return response.StatusCode != HttpStatusCode.OK ? throw new Exception(await response.Content.ReadAsStringAsync())
                : JsonConvert.DeserializeObject<LaundryRecordAddResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}
