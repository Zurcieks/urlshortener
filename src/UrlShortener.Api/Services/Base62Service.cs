using System.Security.Cryptography;

namespace UrlShortener.Api.Services;

public class Base62Service
{
    private const string Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const int Length = 7;

    public string Generate()
    {
        var result = new char[Length];
        for (var i = 0; i < Length; i++)
        {
            result[i] = Chars[RandomNumberGenerator.GetInt32(Chars.Length)];
        }
        return new string(result);
    }

}
