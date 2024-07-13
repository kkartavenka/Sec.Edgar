using System;
using System.IO;
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
    internal class SubmissionProvider : BaseProvider
    {
        internal SubmissionProvider(ICikProvider cikProvider, ILogger logger, CancellationToken ctx) : base(cikProvider,
            logger, ctx)
        {
        }

        internal async Task<Submission> GetAll(string identifier)
        {
            Logger?.LogInformation($"{GetLogPrefix()}: invoked for {nameof(identifier)} {identifier}");
            var cik = await CikProvider.GetFirstAsync(identifier);
            return await GetImplementation(cik);
        }

        internal async Task<Submission> GetAll(int identifier)
        {
            Logger?.LogInformation($"{GetLogPrefix()}: invoked for {nameof(identifier)} {identifier}");
            var cik = await CikProvider.GetFirstAsync(identifier);
            return await GetImplementation(cik);
        }

        private async Task<Submission> GetImplementation(string cikStr)
        {
            var uri = GetUri($"CIK{cikStr}.json");
            Logger?.LogInformation($"{GetLogPrefix()}: Requested CIK: {cikStr}, Uri: {uri.AbsoluteUri}");
            var response = await HttpClientWrapper.GetStreamAsync(uri, Ctx);
            return await TryDeserialize(response);
        }

        private async Task<Submission> TryDeserialize(Stream stream)
        {
            var rootObject = 
#if NET6_0_OR_GREATER
                await JsonSerializer.DeserializeAsync<SubmissionRootJsonDto>(stream, cancellationToken: Ctx);
#elif NETSTANDARD2_0
                JsonConvert.DeserializeObject<SubmissionRootJsonDto>(stream.GetString());
#endif
            if (rootObject?.CentralIndexKey is null)
            {
                return null;
            }

            var returnVar = new Submission(rootObject, CikProvider);
            returnVar.AddFiles(rootObject.Files.RecentFiles);

            foreach (var file in rootObject.Files.Files)
            {
                var responseStream = await HttpClientWrapper.GetStreamAsync(GetUri(file.Name), Ctx);
                var fileObject =
#if NET6_0_OR_GREATER
                    await JsonSerializer.DeserializeAsync<FilingRecentModelJsonDto>(responseStream, cancellationToken: Ctx);
#elif NETSTANDARD2_0
                    JsonConvert.DeserializeObject<FilingRecentModelJsonDto>(responseStream.GetString());
#endif
                returnVar.AddFiles(fileObject);
            }

            return returnVar;
        }

        private Uri GetUri(string file)
        {
            return new Uri($"https://data.sec.gov/submissions/{file}");
        }

        private static string GetLogPrefix([CallerMemberName] string caller = "")
        {
            return $"{nameof(SubmissionProvider)}::{caller}";
        }
    }
}