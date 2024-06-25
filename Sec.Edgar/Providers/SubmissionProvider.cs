using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Models;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Providers;

internal class SubmissionProvider : BaseProvider
{
    internal SubmissionProvider(ICikProvider cikProvider, ILogger? logger, CancellationToken ctx) : base(cikProvider, logger, ctx) { }

    internal async Task<Submission?> GetAll(string identifier)
    {
        Logger?.LogInformation($"{GetLogPrefix()}: invoked for {nameof(identifier)} {identifier}");
        var cik = await CikProvider.GetFirstAsync(identifier);
        return await GetImplementation(cik);
    }
    
    internal async Task<Submission?> GetAll(int identifier)
    {
        Logger?.LogInformation($"{GetLogPrefix()}: invoked for {nameof(identifier)} {identifier}");
        var cik = await CikProvider.GetFirstAsync(identifier);
        return await GetImplementation(cik);
    }

    private async Task<Submission?> GetImplementation(string cikStr)
    {
        var uri = GetUri($"CIK{cikStr}.json");
        Logger?.LogInformation($"{GetLogPrefix}: Requested CIK: {cikStr}, Uri: {uri.AbsoluteUri}");
        var response = await HttpClientWrapper.GetStreamAsync(uri, Ctx);
        return await TryDeserialize(response);
    }

    private async Task<Submission?> TryDeserialize(Stream stream)
    {
        var rootObject = await JsonSerializer.DeserializeAsync<SubmissionRoot>(stream, cancellationToken: Ctx);
        if (rootObject?.CentralIndexKey is null)
        {
            return null;
        }

        var returnVar = new Submission(rootObject, CikProvider);
        returnVar.AddFiles(rootObject.Files.RecentFiles);
        
        foreach (var file in rootObject.Files.Files)
        {
            var responseStream = await HttpClientWrapper.GetStreamAsync(GetUri(file.Name), Ctx);
            var fileObject = await JsonSerializer.DeserializeAsync<FilingRecentModel>(responseStream, cancellationToken: Ctx);
            returnVar.AddFiles(fileObject);
        }

        return returnVar;
    }
    
    private Uri GetUri(string file) => new($"https://data.sec.gov/submissions/{file}");
    
    private static string GetLogPrefix([CallerMemberName] string caller = "") => $"{nameof(SubmissionProvider)}::{caller}";
}