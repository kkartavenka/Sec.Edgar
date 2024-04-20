using System.Net;
using System.Text.Json;
using Moq;
using Moq.Protected;
using Sec.Edgar.CikProviders;

namespace Sec.Edgar.Tests;

public class CikProviderTests
{
    private readonly string _jsonContent;
    private readonly HttpClient? _httpClient;
    
    public CikProviderTests()
    {
        _jsonContent = File.ReadAllText("Data/company_tickers.json");
        
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        mockMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
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
        var identifierInt = 789019;
        var expectedIdentifier = "0000789019";
        
        var sut = new CikJsonProvider(_httpClient, 10, true, "Data/company_tickers.json", CancellationToken.None);
        Assert.Multiple(async () =>
        {
            Assert.That(await sut.GetAsync(identifierInt), Is.EqualTo(expectedIdentifier));
            Assert.That(await sut.GetAsync(identifierInt.ToString()), Is.EqualTo(expectedIdentifier));
            Assert.That(await sut.GetAsync("msft"), Is.EqualTo(expectedIdentifier));
            Assert.That(await sut.GetAsync("MICROSOFT CORP"), Is.EqualTo(expectedIdentifier));
            Assert.That(await sut.GetAsync("MICROSOFT"), Is.EqualTo(expectedIdentifier));
        });
    }

    [Test]
    public async Task JsonMissingProperty()
    {
        var identifierInt = new Random().NextInt64(1_000_000).ToString();
        
        var sut = new CikJsonProvider(_httpClient, 10, true, "Data/wrong_format.json", CancellationToken.None);
        var result = await sut.GetAsync(identifierInt);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(string.Empty));
            Assert.That(sut.Exceptions.Values.Count(x => x.GetType() == typeof(JsonException)), Is.EqualTo(1));
        });
    }
}