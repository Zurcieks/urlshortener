namespace UrlShortener.Api.Entities;

public class ShortUrl
{
    public Guid Id { get; set; }
    public string OriginalUrl { get; set; } = default!;
    public string ShortCode { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

