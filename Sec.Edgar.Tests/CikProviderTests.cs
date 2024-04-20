using System.Net;
using System.Text.Json;
using Moq;
using Moq.Protected;
using Sec.Edgar.CikProviders;

namespace Sec.Edgar.Tests;

public class CikProviderTests
{
    private readonly HttpClient? _httpClient;
    
    public CikProviderTests()
    {
        var jsonContent = File.ReadAllText("Data/company_tickers.json");
        
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        mockMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonContent)
            });

        _httpClient = new(mockMessageHandler.Object);
    }
    
    [Test]
    public void JsonSource()
    {
        const int identifierInt = 789019;
        const string expectedIdentifier = "0000789019";
        
        var sut = new CikJsonProvider(_httpClient, 10, true, "Data/company_tickers.json", CancellationToken.None);
        Assert.Multiple(async () =>
        {
            Assert.That(await sut.GetAsync(identifierInt), Is.EqualTo(expectedIdentifier));
            Assert.That(await sut.GetAsync(identifierInt.ToString()), Is.EqualTo(expectedIdentifier));
            Assert.That(await sut.GetAsync("msft"), Is.EqualTo(expectedIdentifier));
            Assert.That(await sut.GetAsync("MICROSOFT CORP"), Is.EqualTo(expectedIdentifier));
            Assert.That(await sut.GetAsync("MICROSOFT"), Is.EqualTo(expectedIdentifier));
        });
        
        var webSut = new CikJsonProvider(_httpClient, 10, true, CikSourceConstants.CikJsonCompanyTicker, CancellationToken.None);
        Assert.Multiple(async () =>
        {
            Assert.That(await webSut.GetAsync(identifierInt), Is.EqualTo(expectedIdentifier));
            Assert.That(await webSut.GetAsync(identifierInt.ToString()), Is.EqualTo(expectedIdentifier));
            Assert.That(await webSut.GetAsync("msft"), Is.EqualTo(expectedIdentifier));
            Assert.That(await webSut.GetAsync("MICROSOFT CORP"), Is.EqualTo(expectedIdentifier));
            Assert.That(await webSut.GetAsync("MICROSOFT"), Is.EqualTo(expectedIdentifier));
        });
    }

    [Test]
    public void JsonMissingProperty()
    {
        var identifierInt = new Random().NextInt64(1_000_000).ToString();
        
        var sut = new CikJsonProvider(_httpClient, 10, true, "Data/wrong_format.json", CancellationToken.None);
        
        Assert.Multiple(async () =>
        {
            Assert.That(await sut.GetAsync(identifierInt), Is.EqualTo(string.Empty));
            Assert.That(sut.Exceptions.Values.Count(x => x.GetType() == typeof(JsonException)), Is.EqualTo(1));
        });
    }

    [Test]
    public void MissingHttpClient()
    {
        var sut = new CikJsonProvider(null, 10, true, CikSourceConstants.CikJsonCompanyTicker, CancellationToken.None);
        
        Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetAsync(0));
        Assert.That(sut.Exceptions.Values.Count(x => x.GetType() == typeof(ArgumentNullException)), Is.EqualTo(1));
    }
}