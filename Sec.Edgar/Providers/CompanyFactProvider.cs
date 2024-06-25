using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Providers;

internal class CompanyFactProvider : BaseProvider
{
    internal CompanyFactProvider(ICikProvider cikProvider, ILogger? logger, CancellationToken ctx) : base(cikProvider, logger, ctx) { }

    internal async Task<CompanyFact?> Get(string identifier)
    {
        Logger?.LogInformation($"{GetLogPrefix()}: invoked for {nameof(identifier)} {identifier}");
        var cik = await CikProvider.GetFirstAsync(identifier);
        return await GetImplementation(cik);
    }
    
    internal async Task<CompanyFact?> Get(int identifier)
    {
        Logger?.LogInformation($"{GetLogPrefix()}: invoked for {nameof(identifier)} {identifier}");
        var cik = await CikProvider.GetFirstAsync(identifier);
        return await GetImplementation(cik);
    }

    private async Task<CompanyFact?> GetImplementation(string cikStr)
    {
        var uri = GetUri($"CIK{cikStr}.json");
        Logger?.LogInformation($"{GetLogPrefix}: Requested CIK: {cikStr}, Uri: {uri.AbsoluteUri}");
        var stream = await HttpClientWrapper.GetStreamAsync(uri, Ctx);
        return await JsonSerializer.DeserializeAsync<CompanyFact>(stream, cancellationToken: Ctx);
    }
    
    private Uri GetUri(string file) => new($"https://data.sec.gov/api/xbrl/companyfacts/{file}");
    
    private static string GetLogPrefix([CallerMemberName] string caller = "") => $"{nameof(CompanyFactProvider)}::{caller}";
}