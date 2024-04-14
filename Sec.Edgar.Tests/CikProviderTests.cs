using System.Net;
using Moq;
using Moq.Protected;
using Sec.Edgar.CikProviders;

namespace Sec.Edgar.Tests;

public class CikProviderTests
{
    private readonly string _jsonContent;
    private readonly HttpClient _httpClient;
    
    public CikProviderTests()
    {
        _jsonContent = File.ReadAllText("Data/company_tickers.json");
        
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        mockMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", It.IsAny<HttpRequestMessage>(),
                It.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_jsonContent)
            });

        _httpClient = new(mockMessageHandler.Object);
    }
    
    [Test]
    public async Task JsonSource()
    {
        var identifierInt = "789019";
        
        var sut = new CikJsonProvider(_httpClient, 10, true, "Data/company_tickers.json", CancellationToken.None);
        var result = await sut.GetAsync(identifierInt);
        
        sut = new CikJsonProvider(_httpClient, 10, true, "Data/wrong_format_company_tickers.json", CancellationToken.None);
        result = await sut.GetAsync(identifierInt);

        sut = new CikJsonProvider(_httpClient, 10, true, CikSourceConstants.CikJsonCompanyTicker, CancellationToken.None);
        result = await sut.GetAsync(identifierInt);
    }
}