using Sec.Edgar.Models;

namespace Sec.Edgar.CikProviders;

public class CikTextProvider : CikBaseProvider
{
    private static HttpClient _httpClient;
    private readonly string _absoluteSourceLocation;
    private readonly SourceType _sourceType;
    
    public CikTextProvider(HttpClient httpClient, int cikIdentifierLength, bool fillCikIdentifierWithZeroes, string absoluteSourceLocation, CancellationToken ctx) : base(cikIdentifierLength, fillCikIdentifierWithZeroes, ctx)
    {
        _httpClient = httpClient;
        _sourceType = GetSourceType(absoluteSourceLocation);
        _absoluteSourceLocation = absoluteSourceLocation;
    }
}