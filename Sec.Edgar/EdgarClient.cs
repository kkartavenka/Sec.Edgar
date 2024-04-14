using RateLimiter;
using Sec.Edgar.CikProviders;

namespace Sec.Edgar;

public class EdgarClient
{
    private readonly TimeLimiter _timeLimiter;
    private readonly ClientInfo _clientInfo;
    private readonly ICikProvider _cikProvider;
    private static readonly HttpClient _httpClient = new();
    private readonly CancellationTokenSource _cts = new(); 
    
    public EdgarClient(ClientInfo clientInfo)
    {
        _clientInfo = clientInfo;
        _timeLimiter = TimeLimiter.GetFromMaxCountByInterval(clientInfo.RateLimit, TimeSpan.FromSeconds(1));
        _cikProvider = clientInfo.ProviderType switch
        {
            CikProviderType.None => new CikEmptyProvider(_clientInfo.CikIdentifierLength,
                _clientInfo.FillCikIdentifierWithZeroes, CancellationToken.None),
            CikProviderType.Json => new CikJsonProvider(_httpClient, _clientInfo.CikIdentifierLength,
                _clientInfo.FillCikIdentifierWithZeroes, _clientInfo.CikSource, _cts.Token),
            CikProviderType.Text => new CikTextProvider(_httpClient, _clientInfo.CikIdentifierLength,
                _clientInfo.FillCikIdentifierWithZeroes, _clientInfo.CikSource, _cts.Token),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    
}