using System.Text.Json;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Providers;

internal class CompanyFactProvider
{
    private readonly ICikProvider _cikProvider;
    private readonly HttpClientWrapper _httpClientWrapper;
    private readonly CancellationToken _ctx;
    
    internal CompanyFactProvider(ICikProvider cikProvider, CancellationToken ctx)
    {
        _cikProvider = cikProvider;
        _ctx = ctx;
        _httpClientWrapper = HttpClientWrapper.GetInitializedInstance();
    }

    internal async Task<CompanyFact?> Get(string identifier)
    {
        var cik = await _cikProvider.GetAsync(identifier);
        var stream = await _httpClientWrapper.GetStreamAsync(GetUri($"CIK{cik}.json"), _ctx);
        return await JsonSerializer.DeserializeAsync<CompanyFact>(stream, cancellationToken: _ctx);
    }
    
    private Uri GetUri(string file) => new($"https://data.sec.gov/api/xbrl/companyfacts/{file}");
}