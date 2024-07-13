using System;
#if NET6_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
#endif
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Sec.Edgar.Enums;

namespace Sec.Edgar
{
    public class ClientInfoBuilder
    {
        private const int MinCikIdentifierLength = 10;

        private const string UserAgentEmailValidationPattern = @"[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        private const string TickerJsonAddress = @"https://www.sec.gov/files/company_tickers.json";
        private readonly Uri _companyTickerUri = new Uri(TickerJsonAddress);

        private readonly Regex _userAgentEmailValidator = new Regex(UserAgentEmailValidationPattern);
        private int _cikIdentifierFixedLength = MinCikIdentifierLength;
        private CikProviderType _cikProviderType = CikProviderType.None;
        private string _cikSource = string.Empty;
        private bool _fillCikIdentifierWithZeroes = true;
        private ILoggerFactory _loggerFactory;
        private int _rateLimit = 10;
        private string _userAgent;

        /// <summary>
        ///     Sample Company Name AdminContact@Sample_company_domain.com
        /// </summary>
        /// <see cref="https://www.sec.gov/os/accessing-edgar-data" />
        /// <param name="agent"></param>
        /// <returns></returns>
        public ClientInfoBuilder WithUserAgent(string agent)
        {
            if (!_userAgentEmailValidator.Match(agent)
                    .Success)
            {
                throw new ArgumentException("Expected email address in the user agent header", nameof(agent));
            }

            _userAgent = agent;
            return this;
        }

        /// <summary>
        ///     Request per second limiter specified by SEC
        /// </summary>
        /// <see cref="https://www.sec.gov/os/accessing-edgar-data" />
        /// <param name="rateLimit">Request per second limit, 10 requests per second as of Apr 1, 2024</param>
        public ClientInfoBuilder WithRateLimit(uint rateLimit)
        {
            if (rateLimit < 1)
            {
                throw new ArgumentException("Expected a value larger than 1", nameof(rateLimit));
            }

            _rateLimit = (int)rateLimit;
            return this;
        }

        public ClientInfoBuilder WithCikIdentifierLength(int cikLength, bool fillWithZeroes = true)
        {
            if (cikLength < MinCikIdentifierLength)
            {
                throw new ArgumentOutOfRangeException(nameof(cikLength), cikLength,
                    $"Expected value larger than {MinCikIdentifierLength}");
            }

            _fillCikIdentifierWithZeroes = fillWithZeroes;
            _cikIdentifierFixedLength = cikLength;
            return this;
        }

        public ClientInfoBuilder WithCikProvider(CikProviderType cikProviderType, string cikSource = "")
        {
            _cikProviderType = cikProviderType;
            _cikSource = cikSource;
            return this;
        }

        public ClientInfoBuilder WithLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }

        public ClientInfo Build()
        {
            if (_userAgent == null)
            {
                throw new Exception("Use WithUserAgent method to setup User Agent header (SEC requirement)");
            }

            return new ClientInfo(
                _loggerFactory,
                rateLimit: _rateLimit,
                userAgent: _userAgent,
                cikSource: _cikSource,
                tickerJsonAddress: _companyTickerUri,
                providerType: _cikProviderType,
                cikIdentifierLength: _cikIdentifierFixedLength,
                fillCikIdentifierWithZeroes: _fillCikIdentifierWithZeroes);
        }
    }
}