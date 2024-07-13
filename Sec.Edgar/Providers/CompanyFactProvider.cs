using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sec.Edgar.CikProviders;
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
    internal class CompanyFactProvider : BaseProvider
    {
        internal CompanyFactProvider(ICikProvider cikProvider, ILogger logger, CancellationToken ctx) : base(cikProvider,
            logger, ctx)
        {
        }

        internal async Task<CompanyFact> Get(string identifier)
        {
            Logger?.LogInformation($"{GetLogPrefix()}: invoked for {nameof(identifier)} {identifier}");
            var cik = await CikProvider.GetFirstAsync(identifier);
            return await GetImplementation(cik);
        }

        internal async Task<CompanyFact> Get(int identifier)
        {
            Logger?.LogInformation($"{GetLogPrefix()}: invoked for {nameof(identifier)} {identifier}");
            var cik = await CikProvider.GetFirstAsync(identifier);
            return await GetImplementation(cik);
        }

        private async Task<CompanyFact> GetImplementation(string cikStr)
        {
            var uri = GetUri($"CIK{cikStr}.json");
            Logger?.LogInformation($"{GetLogPrefix()}: Requested CIK: {cikStr}, Uri: {uri.AbsoluteUri}");
            var stream = await HttpClientWrapper.GetStreamAsync(uri, Ctx);
            var edgarDto =
#if NET6_0_OR_GREATER
                await JsonSerializer.DeserializeAsync<CompanyFactJsonDto>(stream, cancellationToken: Ctx);
#elif NETSTANDARD2_0
                JsonConvert.DeserializeObject<CompanyFactJsonDto>(stream.GetString());
#endif
            return new CompanyFact(edgarDto);
        }

        private Uri GetUri(string file)
        {
            return new Uri($"https://data.sec.gov/api/xbrl/companyfacts/{file}");
        }

        private static string GetLogPrefix([CallerMemberName] string caller = "")
        {
            return $"{nameof(CompanyFactProvider)}::{caller}";
        }
    }
}