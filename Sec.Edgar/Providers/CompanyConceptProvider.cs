using System.Text.Json;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Enums;
using Sec.Edgar.Models;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Providers;

internal class CompanyConceptProvider : BaseProvider
{
    internal CompanyConceptProvider(ICikProvider cikProvider, CancellationToken ctx) : base(cikProvider, ctx) { }

    internal async Task<CompanyConcept?> Get(string identifier, Taxonomy taxonomy, string xbrlTag)
    {
        var cik = await CikProvider.GetAsync(identifier);
        return await GetImplementation(cik, taxonomy, xbrlTag);
    }
    
    internal async Task<CompanyConcept?> Get(int identifier, Taxonomy taxonomy, string xbrlTag)
    {
        var cik = await CikProvider.GetAsync(identifier);
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
        
        var stream = await HttpClientWrapper.GetStreamAsync(source, Ctx);
        return await JsonSerializer.DeserializeAsync<CompanyConcept>(stream, cancellationToken: Ctx);
    }
    
    private Uri GetUri(string cik, string taxonomy, string xbrlTag) => new($"https://data.sec.gov/api/xbrl/companyconcept/CIK{cik}/{taxonomy}/{xbrlTag}.json");
}