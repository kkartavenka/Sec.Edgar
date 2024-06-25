using System.Net;
using System.Runtime.CompilerServices;
using ComposableAsync;
using Microsoft.Extensions.Logging;
using RateLimiter;

namespace Sec.Edgar;

internal sealed class HttpClientWrapper
{
    private static ILogger? _logger;
    private static readonly object Lock = new();
    private static HttpClient? _httpClient = new();
    private static HttpClientWrapper? _instance;
    private static TimeLimiter? _timeLimiter;
    
    private HttpClientWrapper(ClientInfo clientInfo, ILogger? logger)
    {
        _logger = logger;
        _timeLimiter = TimeLimiter.GetFromMaxCountByInterval(clientInfo.RateLimit, TimeSpan.FromSeconds(1));
        _logger?.LogInformation($"{GetLogPrefix()}: created {nameof(TimeLimiter)} with {clientInfo.RateLimit} per second");
        
        _httpClient = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        });
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", clientInfo.UserAgent);
        _httpClient.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate");
        _logger?.LogInformation($"{GetLogPrefix()}: created {nameof(HttpClient)} with user-agent: {_httpClient.DefaultRequestHeaders.UserAgent}, with accept-encoding: {_httpClient.DefaultRequestHeaders.AcceptEncoding} and auto-decompression {DecompressionMethods.GZip | DecompressionMethods.Deflate}");
    }

    internal static HttpClientWrapper GetInstance(ClientInfo clientInfo)
    {
        if (_instance is not null)
        {
            return _instance;
        }
        
        lock (Lock)
        {
            _instance ??= new HttpClientWrapper(clientInfo, clientInfo.GetLogger<HttpClientWrapper>());
        }

        return _instance;
    }

    internal static HttpClientWrapper GetInitializedInstance([CallerMemberName] string caller = "")
    {
        if (_instance is null)
        {
            throw new ArgumentNullException(nameof(_instance), $"Initialize instance via {nameof(GetInstance)}");
        }
        
        return _instance;
    }

    internal async Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken ctx, [CallerMemberName] string caller = "")
    {
        if (_httpClient is null)
        {
            throw new ArgumentNullException(nameof(_instance), $"Initialize instance via {nameof(GetInstance)}");
        }
        
        await _timeLimiter;
        return await _httpClient.GetAsync(uri, ctx);
    }
    
    internal async Task<Stream> GetStreamAsync(Uri uri, CancellationToken ctx, [CallerMemberName] string caller = "")
    {
        if (_httpClient is null)
        {
            throw new ArgumentNullException(nameof(_instance), $"Initialize instance via {nameof(GetInstance)}");
        }
        
        await _timeLimiter;
        return await _httpClient.GetStreamAsync(uri, ctx);
    }

    internal Func<Uri, CancellationToken, Task<HttpResponseMessage>> GetHandler() =>
        async (uri, token) => await GetAsync(uri, token);
    
    internal Func<Uri, CancellationToken, Task<Stream>> GetStreamHandler() =>
        async (uri, token) => await GetStreamAsync(uri, token);
    
    private static string GetLogPrefix([CallerMemberName] string caller = "") => $"{nameof(HttpClientWrapper)}::{caller}";
}