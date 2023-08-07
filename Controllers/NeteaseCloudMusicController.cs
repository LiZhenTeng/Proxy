using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Proxy.IServices;
using Proxy.Models;

namespace Proxy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ResponseCache(Duration = 90)]
    public class NeteaseCloudMusicController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFetch _fetch;
        public NeteaseCloudMusicController(IConfiguration configuration, IFetch fetch)
        {
            _configuration = configuration;
            _fetch = fetch;
        }
        [HttpGet("/netease-cloud-music/{path}/{id:int}")]
        public async Task<IActionResult> GetByPathAndId(string path, int id, [FromQuery] NeteaseCloudMusic model)
        {
            var response = await Get($"{path}/{id}", model);
            if (response != null)
                return Ok(response);
            return NotFound();
        }
        [HttpGet("/netease-cloud-music/{path}/{url}")]
        public async Task<IActionResult> GetByPathAndUrl(string path, string url, [FromQuery] NeteaseCloudMusic model)
        {
            var response = await Get($"{path}/{url}", model);
            if (response != null)
                return Ok(response);
            return NotFound();
        }
        [HttpGet("/netease-cloud-music/{path}/{id:int}/{url}")]
        public async Task<IActionResult> GetByPathAndIdAndUrl(string path, int id, string url, [FromQuery] NeteaseCloudMusic model)
        {
            var response = await Get($"{path}/{id}/{url}", model);
            if (response != null)
                return Ok(response);
            return NotFound();
        }
        [HttpGet("/netease-cloud-music/{path}/{media}/{url}")]
        public async Task<IActionResult> GetByPathAndMediaAndUrl(string path, string media, string url, [FromQuery] NeteaseCloudMusic model)
        {
            var response = await Get($"{path}/{media}/{url}", model);
            if (response != null)
                return Ok(response);
            return NotFound();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<object?> Get(string path, NeteaseCloudMusic model)
        {
            var query = JsonSerializer.Serialize<NeteaseCloudMusic>(model, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
            return _fetch.FetchNeteaseCloudMusic($"{string.Join("-", path.Split("/"))}-{model.GetKey()}", $"{_configuration.GetSection("NeteaseCloudMusic:API_BASEURL").Value}/{path}", query);
        }
    }
}