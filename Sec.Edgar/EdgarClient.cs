using System;
using System.Threading;
using System.Threading.Tasks;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Enums;
using Sec.Edgar.Models;
using Sec.Edgar.Providers;

namespace Sec.Edgar
{
    public class EdgarClient
    {
        private readonly CompanyConceptProvider _conceptProvider;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly CompanyFactProvider _factProvider;
        private readonly SubmissionProvider _submissionProvider;

        public EdgarClient(ClientInfo clientInfo)
        {
            var httpClientWrapper = HttpClientWrapper.GetInstance(clientInfo);
            
            ICikProvider cikProvider;
            switch (clientInfo.ProviderType)
            {
                case CikProviderType.None:
                    cikProvider = new CikEmptyProvider(clientInfo.GetLogger<CikEmptyProvider>(),
                        clientInfo.CikIdentifierLength,
                        clientInfo.FillCikIdentifierWithZeroes, CancellationToken.None);
                    break;
                case CikProviderType.Json:
                    cikProvider = new CikJsonProvider(clientInfo.GetLogger<CikJsonProvider>(),
                        httpClientWrapper.GetStreamHandler(),
                        clientInfo.CikIdentifierLength,
                        clientInfo.FillCikIdentifierWithZeroes, clientInfo.CikSource, _cts.Token);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(clientInfo.ProviderType), clientInfo.ProviderType,
                        "Unsupported provider");
            }

            _submissionProvider =
                new SubmissionProvider(cikProvider, clientInfo.GetLogger<SubmissionProvider>(), _cts.Token);
            _factProvider = new CompanyFactProvider(cikProvider, clientInfo.GetLogger<CompanyFactProvider>(), _cts.Token);
            _conceptProvider =
                new CompanyConceptProvider(cikProvider, clientInfo.GetLogger<CompanyConceptProvider>(), _cts.Token);
        }

        public async Task<Submission> GetAllSubmissions(string identifier)
        {
            return await _submissionProvider.GetAll(identifier);
        }

        public async Task<Submission> GetAllSubmissions(int cik)
        {
            return await _submissionProvider.GetAll(cik);
        }

        public async Task<CompanyFact> GetCompanyFacts(string identifier)
        {
            return await _factProvider.Get(identifier);
        }

        public async Task<CompanyFact> GetCompanyFacts(int identifier)
        {
            return await _factProvider.Get(identifier);
        }

        public async Task<CompanyConcept> GetCompanyConcept(string identifier, Taxonomy taxonomy, string xbrlTag)
        {
            return await _conceptProvider.Get(identifier, taxonomy, xbrlTag);
        }

        public async Task<CompanyConcept> GetCompanyConcept(int identifier, Taxonomy taxonomy, string xbrlTag)
        {
            return await _conceptProvider.Get(identifier, taxonomy, xbrlTag);
        }
    }
}