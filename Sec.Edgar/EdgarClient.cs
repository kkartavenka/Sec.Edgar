using Sec.Edgar.CikProviders;
using Sec.Edgar.Enums;
using Sec.Edgar.Models;
using Sec.Edgar.Models.Edgar;
using Sec.Edgar.Providers;

namespace Sec.Edgar;

public class EdgarClient
{
    private readonly CancellationTokenSource _cts = new();
    private readonly SubmissionProvider _submissionProvider;
    private readonly CompanyFactProvider _factProvider;
    private readonly CompanyConceptProvider _conceptProvider;

    public EdgarClient(ClientInfo clientInfo)
    {
        var httpClientWrapper = HttpClientWrapper.GetInstance(clientInfo);
        ICikProvider cikProvider = clientInfo.ProviderType switch
        {
            CikProviderType.None => new CikEmptyProvider(clientInfo.GetLogger<CikEmptyProvider>(), clientInfo.CikIdentifierLength,
                clientInfo.FillCikIdentifierWithZeroes, CancellationToken.None),
            CikProviderType.Json => new CikJsonProvider(clientInfo.GetLogger<CikJsonProvider>(), httpClientWrapper.GetStreamHandler(),
                clientInfo.CikIdentifierLength,
                clientInfo.FillCikIdentifierWithZeroes, clientInfo.CikSource, _cts.Token),

            //CikProviderType.Text => new CikTextProvider(httpClientWrapper.GetStreamHandler(), clientInfo.CikIdentifierLength,
            //    clientInfo.FillCikIdentifierWithZeroes, clientInfo.CikSource, _cts.Token),
            _ => throw new ArgumentOutOfRangeException(nameof(clientInfo.ProviderType), clientInfo.ProviderType,
                "Unsupported provider")
        };
        
        _submissionProvider = new SubmissionProvider(cikProvider, clientInfo.GetLogger<SubmissionProvider>(), _cts.Token);
        _factProvider = new CompanyFactProvider(cikProvider, clientInfo.GetLogger<CompanyFactProvider>(), _cts.Token);
        _conceptProvider = new CompanyConceptProvider(cikProvider, clientInfo.GetLogger<CompanyConceptProvider>(), _cts.Token);
    }

    public async Task<Submission?> GetAllSubmissions(string identifier) => await _submissionProvider.GetAll(identifier);
    
    public async Task<Submission?> GetAllSubmissions(int cik) => await _submissionProvider.GetAll(cik);

    public async Task<CompanyFact?> GetCompanyFacts(string identifier) => await _factProvider.Get(identifier);
    
    public async Task<CompanyFact?> GetCompanyFacts(int identifier) => await _factProvider.Get(identifier);

    public async Task<CompanyConcept?> GetCompanyConcept(string identifier, Taxonomy taxonomy, string xbrlTag) =>
        await _conceptProvider.Get(identifier, taxonomy, xbrlTag);
    
    public async Task<CompanyConcept?> GetCompanyConcept(int identifier, Taxonomy taxonomy, string xbrlTag) =>
        await _conceptProvider.Get(identifier, taxonomy, xbrlTag);
}