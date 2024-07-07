using Newtonsoft.Json;
using System.Text;

namespace FullStackApplication.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<T>>(content);
        }

        public async Task PostAsync<T>(string uri, T item)
        {
            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task PutAsync<T>(string uri, T item)
        {
            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();
        }
    }
}
