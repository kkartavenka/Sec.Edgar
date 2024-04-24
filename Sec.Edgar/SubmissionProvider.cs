namespace Sec.Edgar;

internal class SubmissionProvider
{
    private readonly ICikProvider _cikProvider;
    private readonly HttpClientWrapper _httpClientWrapper;
    private readonly CancellationToken _ctx;
    
    internal SubmissionProvider(ICikProvider cikProvider, CancellationToken ctx)
    {
        _cikProvider = cikProvider;
        _httpClientWrapper = HttpClientWrapper.GetInitializedInstance();
    }

    internal async Task Get(string identifier)
    {
        var cik = await _cikProvider.GetAsync(identifier);
        var response = await _httpClientWrapper.GetAsync(GetUri(cik), _ctx);
    }

    private Uri GetUri(string cik) => new Uri($"https://data.sec.gov/submissions/{cik}.json");
}