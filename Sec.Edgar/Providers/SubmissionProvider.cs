using System.Text.Json;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Models;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Providers;

internal class SubmissionProvider
{
    private readonly ICikProvider _cikProvider;
    private readonly HttpClientWrapper _httpClientWrapper;
    private readonly CancellationToken _ctx;
    
    internal SubmissionProvider(ICikProvider cikProvider, CancellationToken ctx)
    {
        _cikProvider = cikProvider;
        _ctx = ctx;
        _httpClientWrapper = HttpClientWrapper.GetInitializedInstance();
    }

    internal async Task<Submission?> GetAll(string identifier)
    {
        var cik = await _cikProvider.GetAsync(identifier);
        var response = await _httpClientWrapper.GetStreamAsync(GetUri($"CIK{cik}.json"), _ctx);
        return await TryDeserialize(response);
    }

    private async Task<Submission?> TryDeserialize(Stream stream)
    {
        var rootObject = await JsonSerializer.DeserializeAsync<SubmissionRoot>(stream, cancellationToken: _ctx);
        if (rootObject?.CentralIndexKey is null)
        {
            return null;
        }

        var returnVar = new Submission(rootObject);
        returnVar.AddFiles(rootObject.Files.RecentFiles);
        
        foreach (var file in rootObject.Files.Files)
        {
            var responseStream = await _httpClientWrapper.GetStreamAsync(GetUri(file.Name), _ctx);
            var fileObject = await JsonSerializer.DeserializeAsync<FilingRecentModel>(responseStream, cancellationToken: _ctx);
            returnVar.AddFiles(fileObject);
        }

        return returnVar;
    }
    
    private Uri GetUri(string file) => new($"https://data.sec.gov/submissions/{file}");
}