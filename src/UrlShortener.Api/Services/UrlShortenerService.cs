using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UrlShortener.Api.Data;
using UrlShortener.Api.Entities;

namespace UrlShortener.Api.Services;

public class UrlShortenerService
{
    private readonly AppDbContext _dbContext;
    private readonly Base62Service _base62Service;
    private readonly IConnectionMultiplexer _redis;

    public UrlShortenerService(AppDbContext dbContext, Base62Service base62Service, IConnectionMultiplexer redis, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _base62Service = base62Service;
        _redis = redis;
    }

    public async Task<string> ShortenAsync(string originalUrl, CancellationToken cancellationToken = default!)
    {
        // Deduplicate URLs: Check if the original URL already exists in the database
        var existing = await _dbContext.ShortUrls.FirstOrDefaultAsync(x => x.OriginalUrl == originalUrl, cancellationToken);

        if (existing is not null)
        {
            return existing.ShortCode;
        }

        string code;
        do
        {
            code = _base62Service.Generate();
        }
        while (await _dbContext.ShortUrls.AnyAsync(x => x.ShortCode == code, cancellationToken));

        var newUrl = new ShortUrl
        {
            ShortCode = code,
            OriginalUrl = originalUrl
        };
        _dbContext.ShortUrls.Add(newUrl);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return code;
    }

    public async Task<string?> ResolveAsync(string code, CancellationToken cancellationToken = default!)
    {
        // Redis cache first - 10:1 read/write ratio means we can significantly reduce database load by caching resolved URLs in Redis

        var cache = _redis.GetDatabase();
        var cached = await cache.StringGetAsync(code);

        if (cached.HasValue)
        {
            return cached.ToString();
        }

        //Fallback to database if not in cache
        var shortUrl = await _dbContext.ShortUrls.AsNoTracking()
        .FirstOrDefaultAsync(x => x.ShortCode == code, cancellationToken);

        if (shortUrl is null)
        {
            return null;
        }

        await cache.StringSetAsync(code, shortUrl.OriginalUrl, TimeSpan.FromHours(1)); // Cache for 1 hour
        return shortUrl.OriginalUrl;
    }
}
