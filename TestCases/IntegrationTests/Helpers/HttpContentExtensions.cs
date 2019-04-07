using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomerApi.IntegrationTests.Helpers
{
    internal static class HttpContentExtensions
    {
        internal static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            string json = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
