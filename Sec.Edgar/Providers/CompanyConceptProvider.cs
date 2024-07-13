using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Enums;
using Sec.Edgar.Models;
using Sec.Edgar.Models.Edgar;

#if NET6_0_OR_GREATER
using System.Text.Json;
#elif NETSTANDARD2_0
using Newtonsoft.Json;
using Sec.Edgar.Extensions;
#endif

namespace Sec.Edgar.Providers
{
    internal class CompanyConceptProvider : BaseProvider
    {
        internal CompanyConceptProvider(ICikProvider cikProvider, ILogger logger, CancellationToken ctx) : base(cikProvider,
            logger, ctx)
        {
        }

        internal async Task<CompanyConcept> Get(string identifier, Taxonomy taxonomy, string xbrlTag)
        {
            Logger?.LogInformation(
                $"{GetLogPrefix()}: invoked for identifier {identifier}, taxonomy {taxonomy.ToString()}, xblr-tag {xbrlTag}");
            var cik = await CikProvider.GetFirstAsync(identifier);
            return await GetImplementation(cik, taxonomy, xbrlTag);
        }

        internal async Task<CompanyConcept> Get(int identifier, Taxonomy taxonomy, string xbrlTag)
        {
            Logger?.LogInformation(
                $"{GetLogPrefix()}: invoked for identifier {identifier}, taxonomy {taxonomy.ToString()}, xblr-tag {xbrlTag}");
            var cik = await CikProvider.GetFirstAsync(identifier);
            return await GetImplementation(cik, taxonomy, xbrlTag);
        }

        private async Task<CompanyConcept> GetImplementation(string cik, Taxonomy taxonomy, string xbrlTag)
        {
            var taxonomyMapping = EnumExtension.GetAttributeMapping<Taxonomy>();
            var taxonomyStr = taxonomyMapping.SingleOrDefault(x => x.Key == taxonomy.ToString())
                .Value;

            if (string.IsNullOrWhiteSpace(taxonomyStr))
            {
                return default;
            }

            var source = GetUri(cik, taxonomyStr, xbrlTag);
            Logger?.LogInformation(
                $"{GetLogPrefix()}: Requested CIK: {cik}, taxonomy: {taxonomy.ToString()}, Uri: {source.AbsoluteUri}");

            var stream = await HttpClientWrapper.GetStreamAsync(source, Ctx);
            var companyConceptJsonDto =
#if NET6_0_OR_GREATER
                await JsonSerializer.DeserializeAsync<CompanyConceptJsonDto>(stream, cancellationToken: Ctx);
#elif NETSTANDARD2_0
                JsonConvert.DeserializeObject<CompanyConceptJsonDto>(stream.GetString());
#endif

            return new CompanyConcept(companyConceptJsonDto);
        }
        
        private Uri GetUri(string cik, string taxonomy, string xbrlTag)
        {
            return new Uri($"https://data.sec.gov/api/xbrl/companyconcept/CIK{cik}/{taxonomy}/{xbrlTag}.json");
        }

        private static string GetLogPrefix([CallerMemberName] string caller = "")
        {
            return $"{nameof(CompanyConceptProvider)}::{caller}";
        }
    }
}