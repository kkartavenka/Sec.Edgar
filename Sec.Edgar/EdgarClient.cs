using Sec.Edgar.CikProviders;
using Sec.Edgar.Models;
using Sec.Edgar.Models.Edgar;
using Sec.Edgar.Providers;

namespace Sec.Edgar;

public class EdgarClient
{
    private readonly ClientInfo _clientInfo;
    private readonly ICikProvider _cikProvider;
    private readonly CancellationTokenSource _cts = new();
    private readonly SubmissionProvider _submissionProvider;
    private readonly CompanyFactProvider _factProvider;

    public EdgarClient(ClientInfo clientInfo)
    {
        _clientInfo = clientInfo;
        var httpClientWrapper = HttpClientWrapper.GetInstance(clientInfo);
        _cikProvider = clientInfo.ProviderType switch
        {
            CikProviderType.None => new CikEmptyProvider(_clientInfo.CikIdentifierLength,
                _clientInfo.FillCikIdentifierWithZeroes, CancellationToken.None),
            CikProviderType.Json => new CikJsonProvider(httpClientWrapper.GetStreamHandler(), _clientInfo.CikIdentifierLength,
                _clientInfo.FillCikIdentifierWithZeroes, _clientInfo.CikSource, _cts.Token),
            CikProviderType.Text => new CikTextProvider(httpClientWrapper.GetStreamHandler(), _clientInfo.CikIdentifierLength,
                _clientInfo.FillCikIdentifierWithZeroes, _clientInfo.CikSource, _cts.Token),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        _submissionProvider = new SubmissionProvider(_cikProvider, _cts.Token);
        _factProvider = new CompanyFactProvider(_cikProvider, _cts.Token);
    }

    public async Task<Submission?> GetAllSubmissions(string identifier) => await _submissionProvider.GetAll(identifier);

    public async Task<CompanyFact?> GetCompanyFacts(string identifier) => await _factProvider.Get(identifier);
}