using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CleanArchitecture.Infrastructure.Extensions
{
    public static class HttpClientExtensions
    {
        private static readonly JsonSerializerOptions _defaultOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static Task<HttpResponseMessage> PostXmlAsync(this HttpClient httpClient, string requestUri, string xml, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            return httpClient.PostAsync(requestUri, new StringContent(xml, Encoding.UTF8, "text/xml"), cancellationToken);
        }

        public static Task<HttpResponseMessage> PostJsonAsync(this HttpClient httpClient, string requestUri, object data, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            var json = JsonSerializer.Serialize(data, _defaultOptions);
            return httpClient.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"), cancellationToken);
        }

        public static Task<HttpResponseMessage> PostJsonAsync(this HttpClient httpClient, string requestUri, object data, JsonSerializerOptions options, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            var json = JsonSerializer.Serialize(data, options);
            return httpClient.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"), cancellationToken);
        }

        public static async Task<TResult?> PostJsonAsync<TResult>(this HttpClient httpClient, string requestUri, object data, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            var json = JsonSerializer.Serialize(data, _defaultOptions);
            var responseMessage = await httpClient.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"), cancellationToken);
            var response = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<TResult>(response, _defaultOptions);
        }

        public static async Task<TResult?> GetJsonAsync<TResult>(this HttpClient httpClient, string requestUri)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            var response = await httpClient.GetStringAsync(requestUri);
            return JsonSerializer.Deserialize<TResult>(response, _defaultOptions);
        }

        public static Task<HttpResponseMessage> PostFormAsync(this HttpClient httpClient, string requestUri, IEnumerable<KeyValuePair<string, string>> data, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            return httpClient.PostAsync(requestUri, new FormUrlEncodedContent(data), cancellationToken);
        }

        public static void AddOrUpdate(this HttpRequestHeaders headers, string name, string value)
        {
            ArgumentNullException.ThrowIfNull(headers);
            if (headers.Contains(name))
                headers.Remove(name);
            headers.Add(name, value);
        }
    }
}
