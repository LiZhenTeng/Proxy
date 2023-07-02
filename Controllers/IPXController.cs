using Microsoft.AspNetCore.Mvc;
using Proxy.IServices;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Proxy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ResponseCache(Duration = 90)]
    public class IPXController : ControllerBase
    {
        private readonly ILogger<IPXController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IFetch _fetch;
        public IPXController(ILogger<IPXController> logger, IFetch fetch, IConfiguration configuration)
        {
            _logger = logger;
            _fetch = fetch;
            _configuration = configuration;
        }
        [HttpGet("/ipx/{path}/tmdb/{imageName}")]
        public async Task<IActionResult> GetTmdbImage(string imageName)
        {
            var (buffer, contentType) = await _fetch.FetchImage(Path.GetFullPath(imageName), _configuration.GetSection("TMDB:TMDB_IMAGE_BASEURL").Value ?? "", imageName);
            if (buffer != null)
                return File(buffer, contentType);
            return NotFound();
        }
        [HttpGet("/ipx/{path}/youtube/{p}/{url}/{imageName}")]
        public async Task<IActionResult> GetYoTuBeImage(string p, string url, string imageName)
        {
            var (buffer, contentType) = await _fetch.FetchImage(Path.GetFullPath(imageName), $"{_configuration.GetSection("TMDB:YOUTUBE_IMAGE_BASEURL").Value}/{p}/{url}", imageName);
            if (buffer != null)
                return File(buffer, contentType);
            return NotFound();
        }
    }
}