namespace UrlShortener.Api.Services;

public class UrlValidator
{
    public bool IsValid(string url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
        (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
}
