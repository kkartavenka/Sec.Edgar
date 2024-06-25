using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Enums;
using Sec.Edgar.Models;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Providers;

internal class CompanyConceptProvider : BaseProvider
{
    internal CompanyConceptProvider(ICikProvider cikProvider, ILogger? logger, CancellationToken ctx) : base(cikProvider, logger, ctx) { }

    internal async Task<CompanyConcept?> Get(string identifier, Taxonomy taxonomy, string xbrlTag)
    {
        Logger?.LogInformation($"{GetLogPrefix()}: invoked for identifier {identifier}, taxonomy {taxonomy.ToString()}, xblr-tag {xbrlTag}");
        var cik = await CikProvider.GetFirstAsync(identifier);
        return await GetImplementation(cik, taxonomy, xbrlTag);
    }
    
    internal async Task<CompanyConcept?> Get(int identifier, Taxonomy taxonomy, string xbrlTag)
    {
        Logger?.LogInformation($"{GetLogPrefix()}: invoked for identifier {identifier}, taxonomy {taxonomy.ToString()}, xblr-tag {xbrlTag}");
        var cik = await CikProvider.GetFirstAsync(identifier);
        return await GetImplementation(cik, taxonomy, xbrlTag);
    }

    private async Task<CompanyConcept?> GetImplementation(string cik, Taxonomy taxonomy, string xbrlTag)
    {
        var taxonomyMapping = EnumExtension.GetAttributeMapping<Taxonomy>();
        var taxonomyStr = taxonomyMapping.SingleOrDefault(x=>x.Key == taxonomy.ToString()).Value;

        if (string.IsNullOrWhiteSpace(taxonomyStr))
        {
            return default;
        }
        
        var source = GetUri(cik, taxonomyStr, xbrlTag);
        Logger?.LogInformation($"{GetLogPrefix}: Requested CIK: {cik}, taxonomy: {taxonomy.ToString()}, Uri: {source.AbsoluteUri}");
        
        var stream = await HttpClientWrapper.GetStreamAsync(source, Ctx);
        return await JsonSerializer.DeserializeAsync<CompanyConcept>(stream, cancellationToken: Ctx);
    }
    
    private Uri GetUri(string cik, string taxonomy, string xbrlTag) => new($"https://data.sec.gov/api/xbrl/companyconcept/CIK{cik}/{taxonomy}/{xbrlTag}.json");
    
    private static string GetLogPrefix([CallerMemberName] string caller = "") => $"{nameof(CompanyConceptProvider)}::{caller}";
}