using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ComposableAsync;
using Microsoft.Extensions.Logging;
using RateLimiter;

namespace Sec.Edgar
{
    internal sealed class HttpClientWrapper
    {
        private static readonly object Lock = new object();
        private static HttpClient _httpClient = new HttpClient();
        private static HttpClientWrapper _instance;
        private static TimeLimiter _timeLimiter;

        private HttpClientWrapper(ClientInfo clientInfo, ILogger logger)
        {
            _timeLimiter = TimeLimiter.GetFromMaxCountByInterval(clientInfo.RateLimit, TimeSpan.FromSeconds(1));
            logger?.LogInformation(
                "{selfInfo}: created {instance} with {rateLimitValue} per second",
                GetLogPrefix(),
                nameof(TimeLimiter),
                clientInfo.RateLimit);

            _httpClient = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", clientInfo.UserAgent);
            _httpClient.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate");
            logger?.LogInformation(
                "{selfInfo}: created HttpClient with user-agent: {userAgent}, with accept-encoding: {acceptEncoding} and auto-decompression {decompression}",
                GetLogPrefix(),
                _httpClient.DefaultRequestHeaders.UserAgent,
                _httpClient.DefaultRequestHeaders.AcceptEncoding,
                DecompressionMethods.GZip | DecompressionMethods.Deflate);
        }

        internal static HttpClientWrapper GetInstance(ClientInfo clientInfo)
        {
            if (_instance != null)
            {
                return _instance;
            }

            lock (Lock)
            {
                _instance = new HttpClientWrapper(clientInfo, clientInfo.GetLogger<HttpClientWrapper>());
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

        internal async Task<Stream> GetStreamAsync(Uri uri, CancellationToken ctx, [CallerMemberName] string caller = "")
        {
            if (_httpClient is null)
            {
                throw new ArgumentNullException(nameof(_instance), $"Initialize instance via {nameof(GetInstance)}");
            }

            await _timeLimiter;
            
#if NET6_0_OR_GREATER
            return await _httpClient.GetStreamAsync(uri, ctx);
#elif NETSTANDARD2_0
            return await _httpClient.GetStreamAsync(uri);
#endif
        }

        internal Func<Uri, CancellationToken, Task<HttpResponseMessage>> GetHandler()
        {
            return async (uri, token) => await GetAsync(uri, token);
        }

        internal Func<Uri, CancellationToken, Task<Stream>> GetStreamHandler()
        {
            return async (uri, token) => await GetStreamAsync(uri, token);
        }

        private static string GetLogPrefix([CallerMemberName] string caller = "")
        {
            return $"{nameof(HttpClientWrapper)}::{caller}";
        }

        private async Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken ctx)
        {
            if (_httpClient is null)
            {
                throw new ArgumentNullException(nameof(_instance), $"Initialize instance via {nameof(GetInstance)}");
            }

            await _timeLimiter;
            return await _httpClient.GetAsync(uri, ctx);
        }
    }
}