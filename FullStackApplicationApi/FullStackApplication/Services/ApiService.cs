using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FullStackApplication.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        //public async Task PostAsync<T>(string uri, T item)
        //{
        //    AddAuthorizationHeader();
        //    var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PostAsync(uri, content);
        //    response.EnsureSuccessStatusCode();
        //}

        public async Task<T> PostAsync<T>(string uri, T data)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public async Task<TResponse> PostAsync<TResponse, TRequest>(string uri, TRequest data)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string uri)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<T>>(content);
        }

        //public async Task PutAsync<T>(string uri, T item)
        //{
        //    AddAuthorizationHeader();
        //    var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PutAsync(uri, content);
        //    response.EnsureSuccessStatusCode();
        //}
        public async Task<T> PutAsync<T>(string uri, T data)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public async Task DeleteAsync(string uri)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();
        }
    }
}
