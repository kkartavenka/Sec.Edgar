using System;
using Microsoft.Extensions.Logging;
using Sec.Edgar.Enums;

namespace Sec.Edgar
{
    public class ClientInfo
    {
        public ClientInfo(
            ILoggerFactory loggerFactory,
            string userAgent,
            int rateLimit,
            Uri tickerJsonAddress,
            int cikIdentifierLength,
            bool fillCikIdentifierWithZeroes,
            CikProviderType providerType,
            string cikSource)
        {
            LoggerFactory = loggerFactory;
            UserAgent = userAgent;
            RateLimit = rateLimit;
            TickerJsonAddress = tickerJsonAddress;
            CikIdentifierLength = cikIdentifierLength;
            FillCikIdentifierWithZeroes = fillCikIdentifierWithZeroes;
            ProviderType = providerType;
            CikSource = cikSource;
        }

        internal ILoggerFactory LoggerFactory { get; }
        public string UserAgent { get; }
        public int RateLimit { get; }
        public Uri TickerJsonAddress { get; private set; }
        public int CikIdentifierLength { get; private set; }
        public bool FillCikIdentifierWithZeroes { get; private set; }
        public CikProviderType ProviderType { get; private set; }
        public string CikSource { get; private set; }

        internal ILogger<T> GetLogger<T>()
        {
            return LoggerFactory?.CreateLogger<T>();
        }
    }
}