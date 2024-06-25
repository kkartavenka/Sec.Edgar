using Microsoft.Extensions.Logging;
using Sec.Edgar.Enums;

namespace Sec.Edgar;

public class ClientInfo
{
    internal ILoggerFactory? LoggerFactory { get; init; }
    public required string UserAgent { get; init; }
    public required int RateLimit { get; init; }
    public Uri? TickerJsonAddress { get; init; }
    public required int CikIdentifierLength {get; init; }
    public required bool FillCikIdentifierWithZeroes { get; init; }
    public required CikProviderType ProviderType { get; init; }
    public required string CikSource { get; init; }

    internal ILogger<T>? GetLogger<T>() => LoggerFactory?.CreateLogger<T>();
}