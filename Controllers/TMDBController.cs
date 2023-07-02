using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Proxy.IServices;
using Proxy.Models;

namespace Proxy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ResponseCache(Duration = 90)]
    public class TMDBController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFetch _fetch;
        public TMDBController(IConfiguration configuration, IFetch fetch)
        {
            _configuration = configuration;
            _fetch = fetch;
        }
        [HttpGet("/tmdb/{path}/{id:int}")]
        public async Task<IActionResult> GetByPathAndId(string path, int id, [FromQuery] TMDB model)
        {
            var response = await Get($"{path}/{id}", model);
            if (response != null)
                return Ok(response);
            return NotFound();
        }
        [HttpGet("/tmdb/{path}/{url}")]
        public async Task<IActionResult> GetByPathAndUrl(string path, string url, [FromQuery] TMDB model)
        {
            var response = await Get($"{path}/{url}", model);
            if (response != null)
                return Ok(response);
            return NotFound();
        }
        [HttpGet("/tmdb/{path}/{id:int}/{url}")]
        public async Task<IActionResult> GetByPathAndIdAndUrl(string path, int id, string url, [FromQuery] TMDB model)
        {
            var response = await Get($"{path}/{id}/{url}", model);
            if (response != null)
                return Ok(response);
            return NotFound();
        }
        [HttpGet("/tmdb/{path}/{media}/{url}")]
        public async Task<IActionResult> GetByPathAndMediaAndUrl(string path, string media, string url, [FromQuery] TMDB model)
        {
            var response = await Get($"{path}/{media}/{url}", model);
            if (response != null)
                return Ok(response);
            return NotFound();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<object?> Get(string path, TMDB model)
        {
            var query = JsonSerializer.Serialize<TMDB>(model, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
            return _fetch.FetchTMDB($"{string.Join("-", path.Split("/"))}-{model.GetKey()}", $"{_configuration.GetSection("TMDB:TMDB_API_BASEURL").Value}/{path}", query);
        }
    }
}