using System.Net;
using System.Text;
using Sec.Edgar.CikProviders;
using Sec.Edgar.Enums;
using Sec.Edgar.Models.Edgar;

namespace Sec.Edgar.Tests;

/// <summary>
///     Covers CIK resolution, including a round trip over real HTTP against a loopback
///     server serving the company_tickers.json shape that SEC publishes.
/// </summary>
public class CikLookupTests
{
    private static string DataPath(string file) =>
        Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", file);

    private static ModelManager LoadedManager(string file = "company_tickers.json")
    {
        var manager = new ModelManager(10, true);
        var json = File.ReadAllText(DataPath(file));
        var parsed = System.Text.Json.JsonSerializer
            .Deserialize<Dictionary<string, EdgarTickerJsonDto>>(json)!;
        manager.LoadData(parsed.Select(x => x.Value).ToList());
        return manager;
    }

    [Test]
    public void ResolvesByTickerCaseInsensitively()
    {
        var manager = LoadedManager();
        Assert.Multiple(() =>
        {
            Assert.That(manager.GetCik("MSFT"), Is.EqualTo("0000789019"));
            Assert.That(manager.GetCik("msft"), Is.EqualTo("0000789019"));
            Assert.That(manager.GetCik(" aapl "), Is.EqualTo("0000320193"));
        });
    }

    [Test]
    public void ResolvesByNumericIdentifier()
    {
        var manager = LoadedManager();
        Assert.Multiple(() =>
        {
            Assert.That(manager.GetCik("789019"), Is.EqualTo("0000789019"));
            Assert.That(manager.GetCik("0000789019"), Is.EqualTo("0000789019"));
            Assert.That(manager.GetCik(789019), Is.EqualTo("0000789019"));
        });
    }

    /// <summary>
    ///     Regression: the name fallback was unreachable because ToList() never returns null,
    ///     so the README's documented "look up by name" never matched.
    /// </summary>
    [Test]
    public void ResolvesByCompanyNameFragment()
    {
        var manager = LoadedManager();
        Assert.Multiple(() =>
        {
            Assert.That(manager.GetCik("MICROSOFT"), Is.EqualTo("0000789019"));
            Assert.That(manager.GetCik("Apple"), Is.EqualTo("0000320193"));
            Assert.That(manager.GetCik("alphabet"), Is.EqualTo("0001652044"));
        });
    }

    /// <summary>
    ///     Regression: a miss used to throw InvalidOperationException from First().
    /// </summary>
    [Test]
    public void ReturnsEmptyStringWhenIdentifierIsUnknown()
    {
        var manager = LoadedManager();
        Assert.Multiple(() =>
        {
            Assert.That(manager.GetCik("ZZZZ"), Is.Empty);
            Assert.That(manager.GetCik("No Such Company"), Is.Empty);
            Assert.That(manager.GetCik(9999999), Is.Empty);
        });
    }

    [Test]
    public void KeepsBothTickersOfOneIssuerAndCollapsesExactDuplicates()
    {
        var manager = LoadedManager("duplicate_company_tickers.json");
        Assert.Multiple(() =>
        {
            Assert.That(manager.GetTickerInfo(789019), Has.Count.EqualTo(2));
            Assert.That(manager.GetCik("MSFT.B"), Is.Not.Empty);
        });
    }

    [Test]
    public void ZeroFillIsHonouredAndNeverTruncates()
    {
        Assert.Multiple(() =>
        {
            Assert.That(new ModelManager(10, true).FillStringWithZeroes("789019"), Is.EqualTo("0000789019"));
            Assert.That(new ModelManager(10, false).FillStringWithZeroes("789019"), Is.EqualTo("789019"));
            Assert.That(new ModelManager(4, true).FillStringWithZeroes("789019"), Is.EqualTo("789019"));
        });
    }

    [Test]
    public async Task ReadsTickerFileFromDiskThroughTheProvider()
    {
        var provider = new CikJsonProvider(null, null, 10, true, DataPath("company_tickers.json"),
            CancellationToken.None);
        Assert.That(await provider.GetFirstAsync("NVDA"), Is.EqualTo("0001045810"));
    }

    /// <summary>
    ///     Drives the real HttpClient path and asserts that the SEC-mandated User-Agent
    ///     actually reaches the wire.
    /// </summary>
    [Test]
    public async Task FetchesTickerFileOverHttpAndSendsTheUserAgent()
    {
        const string agent = "Sec.Edgar Tests tests@example.com";
        var payload = File.ReadAllText(DataPath("company_tickers.json"));

        using var listener = new HttpListener();
        var port = 18100 + Random.Shared.Next(300);
        listener.Prefixes.Add($"http://127.0.0.1:{port}/");
        listener.Start();

        string? seenAgent = null;
        var serving = Task.Run(async () =>
        {
            var ctx = await listener.GetContextAsync();
            seenAgent = ctx.Request.Headers["User-Agent"];
            var bytes = Encoding.UTF8.GetBytes(payload);
            ctx.Response.ContentLength64 = bytes.Length;
            await ctx.Response.OutputStream.WriteAsync(bytes);
            ctx.Response.Close();
        });

        var clientInfo = new ClientInfoBuilder()
            .WithUserAgent(agent)
            .WithCikProvider(CikProviderType.Json, $"http://127.0.0.1:{port}/company_tickers.json")
            .Build();

        var wrapper = HttpClientWrapper.GetInstance(clientInfo);
        var provider = new CikJsonProvider(null, wrapper.GetStreamHandler(), 10, true,
            $"http://127.0.0.1:{port}/company_tickers.json", CancellationToken.None);

        var cik = await provider.GetFirstAsync("GOOGL");
        await serving;
        listener.Stop();

        Assert.Multiple(() =>
        {
            Assert.That(cik, Is.EqualTo("0001652044"));
            Assert.That(seenAgent, Is.EqualTo(agent));
        });
    }
}
