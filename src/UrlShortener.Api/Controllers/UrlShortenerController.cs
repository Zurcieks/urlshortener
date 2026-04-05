using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using UrlShortener.Api.Services;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlShortenerController : ControllerBase
{
    private readonly UrlShortenerService _shortener;
    private readonly UrlValidator _validator;

    public UrlShortenerController(UrlShortenerService shortener, UrlValidator validator)
    {
        _shortener = shortener;
        _validator = validator;
    }
    [HttpPost("shorten")]
    [EnableRateLimiting("shorten")]
    public async Task<IActionResult> Shorten([FromBody] ShortenRequest request, CancellationToken cancellationToken)
    {
        if (!_validator.IsValid(request.OriginalUrl))
        {
            return BadRequest(new { error = "Provide valid URL (http/https)." });
        }

        var code = await _shortener.ShortenAsync(request.OriginalUrl, cancellationToken);
        var shortUrl = $"{Request.Scheme}://{Request.Host}/{code}"; // Return full URL for better UX, even though the client can construct it from the code and base URL if they want to.
        return Ok(new { shortUrl });
    }
}

public record ShortenRequest(string OriginalUrl);
