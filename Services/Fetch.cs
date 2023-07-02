using System.Text.Json;
using Proxy.IServices;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Proxy.Services;

public class Fetch : IFetch
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _clientFactory;
    private readonly ICache _cache;
    public Fetch(IConfiguration configuration, IHttpClientFactory clientFactory, ICache cache)
    {
        _configuration = configuration;
        _clientFactory = clientFactory;
        _cache = cache;
    }
    public async Task<(byte[]?, string)> FetchImage(string key, string url, string imageName)
    {
        var extension = Path.GetExtension(imageName).Replace(".", "");

        var b = _cache.TryGetValue<byte[]?>(key, out byte[]? value);
        if (b && value != null)
        {
            return (value, $"image/{extension}");
        }
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{url}/{imageName}");
        var client = _clientFactory.CreateClient();
        var response = await client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            response.Content.Headers.ContentType = new MediaTypeHeaderValue($"image/{extension}");
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var buffer = new byte[responseStream.Length];
            await responseStream.ReadAsync(buffer);
            var r = _cache.Set(key, buffer);
            return (r, $"image/{extension}");
        }
        return (null, $"image/{extension}");
    }
    public async Task<object?> FetchTMDB(string key, string path, string query)
    {
        var value = _cache.Get(key);
        if (value != null)
        {
            return value;
        }
        JObject j = JObject.Parse(query);
        j.Add("api_key", _configuration.GetSection("TMDB:TMDB_API_KEY").Value ?? "");
        var queryString = string.Join("&", j.Properties().Select(x => $"{x.Name}={x.Value}").ToArray());
        
        using var request = new HttpRequestMessage(HttpMethod.Get, j.Count > 1 ? $"{path}?{queryString}" : path)
        {
            Headers =
                {
                    { Microsoft.Net.Http.Headers.HeaderNames.Accept, "application/json" }
                }
        };
        var client = _clientFactory.CreateClient();
        var response = await client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<object>(responseStream);
            if (content != null)
                return _cache.Set(key, content);
        }
        return null;

    }
}
