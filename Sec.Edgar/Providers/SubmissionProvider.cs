using System.Text.Json;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Models;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Providers;

internal class SubmissionProvider : BaseProvider
{
    internal SubmissionProvider(ICikProvider cikProvider, CancellationToken ctx) : base(cikProvider, ctx) { }

    internal async Task<Submission?> GetAll(string identifier)
    {
        var cik = await CikProvider.GetAsync(identifier);
        return await GetImplementation(cik);
    }
    
    internal async Task<Submission?> GetAll(int identifier)
    {
        var cik = await CikProvider.GetAsync(identifier);
        return await GetImplementation(cik);
    }

    private async Task<Submission?> GetImplementation(string cikStr)
    {
        var response = await HttpClientWrapper.GetStreamAsync(GetUri($"CIK{cikStr}.json"), Ctx);
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
}