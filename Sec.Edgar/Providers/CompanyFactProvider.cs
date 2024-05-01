using System.Text.Json;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Providers;

internal class CompanyFactProvider : BaseProvider
{
    internal CompanyFactProvider(ICikProvider cikProvider, CancellationToken ctx) : base(cikProvider, ctx) { }

    internal async Task<CompanyFact?> Get(string identifier)
    {
        var cik = await CikProvider.GetAsync(identifier);
        return await GetImplementation(cik);
    }
    
    internal async Task<CompanyFact?> Get(int identifier)
    {
        var cik = await CikProvider.GetAsync(identifier);
        return await GetImplementation(cik);
    }

    private async Task<CompanyFact?> GetImplementation(string cikStr)
    {
        var stream = await HttpClientWrapper.GetStreamAsync(GetUri($"CIK{cikStr}.json"), Ctx);
        return await JsonSerializer.DeserializeAsync<CompanyFact>(stream, cancellationToken: Ctx);
    }
    
    private Uri GetUri(string file) => new($"https://data.sec.gov/api/xbrl/companyfacts/{file}");
}