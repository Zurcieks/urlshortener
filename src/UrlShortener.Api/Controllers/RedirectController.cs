using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Services;

namespace UrlShortener.Api.Controllers;

[ApiController]
public class RedirectController : ControllerBase
{
    private readonly UrlShortenerService _shortener;

    public RedirectController(UrlShortenerService shortener)
    {
        _shortener = shortener;
    }

    [HttpGet("/{code}")]
    public async Task<IActionResult> Resolve(string code, CancellationToken cancellationToken)
    {
        var longUrl = await _shortener.ResolveAsync(code, cancellationToken);
        return longUrl is null
            ? NotFound(new { error = "URL not found." })
            : Redirect(longUrl);
    }
}
