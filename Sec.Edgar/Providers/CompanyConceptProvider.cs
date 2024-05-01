using Sec.Edgar.CikProviders;
using Sec.Edgar.Models;

namespace Sec.Edgar.Providers;

internal class CompanyConceptProvider : BaseProvider
{
    internal CompanyConceptProvider(ICikProvider cikProvider, CancellationToken ctx) : base(cikProvider, ctx) { }

    internal async Task Get(string identifier, Taxonomy taxonomy, string xbrlTag)
    {
        
    }
    
    private Uri GetUri(string cik, Taxonomy taxonomy, string xbrlTag) => new($"https://data.sec.gov/api/xbrl/companyconcept/CIK{cik}/{taxonomy}/{xbrlTag}.json");
}